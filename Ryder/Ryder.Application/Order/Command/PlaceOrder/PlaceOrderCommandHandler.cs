using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Command.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, IResult<Guid>>
    {
        private readonly ApplicationContext _context;

        public PlaceOrderCommandHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
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
                },
                DropOffLocation = new Address
                {
                    City = request.PickUpLocation.City,
                    State = request.PickUpLocation.State,
                    PostCode = request.PickUpLocation.PostCode,
                    Longitude = request.PickUpLocation.Longitude,
                    Latitude = request.PickUpLocation.Latitude,
                    Country = request.PickUpLocation.Country,
                },
                PickUpPhoneNumber = request.PickUpPhoneNumber,
                ReferenceNumber = request.ReferenceNumber,
                Amount = request.Amount,
                RiderId = request.RiderId,
                Status = OrderStatus.OrderPlaced
            };

            try
            {
                await _context.Orders.AddAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(order.Id, "Order placed successfully");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Fail(ex.Message);
            }
        }
    }
}