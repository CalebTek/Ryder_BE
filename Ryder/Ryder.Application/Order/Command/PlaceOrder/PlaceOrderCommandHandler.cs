using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ryder.Application.Common.Hubs;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Ryder.Application.Order.Command.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, IResult<Guid>>
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public PlaceOrderCommandHandler(ApplicationContext context, UserManager<AppUser> userManager,
            IHubContext<NotificationHub> notificationHub)
        {
            _context = context;
            _userManager = userManager;
            _notificationHub = notificationHub;
        }

        public async Task<IResult<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await _userManager.FindByIdAsync(request.AppUserId.ToString());

                if (currentUser == null)
                {
                    return Result<Guid>.Fail("User not found");
                }

                var order = new Domain.Entities.Order
                {
                    Id = Guid.NewGuid(),
                    PickUpLocation = new Address
                    {
                        City = request.PickUpLocation.City,
                        State = request.PickUpLocation.State,
                        PostCode = request.PickUpLocation.PostCode,
                        Longitude = request.PickUpLocation.Longitude,
                        Latitude = request.PickUpLocation.Latitude,
                        Country = request.PickUpLocation.Country,
                        AddressDescription = request.PickUpLocation.AddressDescription,
                    },
                    DropOffLocation = new Address
                    {
                        City = request.PickUpLocation.City,
                        State = request.PickUpLocation.State,
                        PostCode = request.PickUpLocation.PostCode,
                        Longitude = request.PickUpLocation.Longitude,
                        Latitude = request.PickUpLocation.Latitude,
                        Country = request.PickUpLocation.Country,
                        AddressDescription = request.DropOffLocation.AddressDescription,
                    },
                    PickUpPhoneNumber = request.PickUpPhoneNumber,
                    PackageDescription = request.PackageDescription,
                    ReferenceNumber = request.ReferenceNumber,
                    Amount = request.Amount,
                    RiderId = Guid.Empty,
                    AppUserId = currentUser.Id,
                    Status = OrderStatus.OrderPlaced,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Email = currentUser.Email,
                    Name = currentUser.FirstName +" "+ currentUser.LastName
                };


                List<string> getAllAvailableRiders = await _context.Riders
                    .Where(row => row.AvailabilityStatus == RiderAvailabilityStatus.Available)
                    .Select(rider => rider.Id.ToString()).ToListAsync();

                if (!getAllAvailableRiders.Any())
                {
                    return Result<Guid>.Fail("No available rider to fufil your order!");
                }

                await _notificationHub.Clients.All.SendAsync("IncomingRequest", getAllAvailableRiders,
                    "You have an incoming request.", cancellationToken: cancellationToken);

                await _context.Orders.AddAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(order.Id, "Order placed successfully");
            }
            catch (Exception)
            {
                return Result<Guid>.Fail("Order not placed");
            }
        }
    }
}