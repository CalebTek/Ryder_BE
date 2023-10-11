using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;

namespace Ryder.Application.Order.Command.DeclineOrder
{
    public class DeclineOrderCommandHandler : IRequestHandler<DeclineOrderCommand, IResult<string>>
    {
        private readonly ApplicationContext _context;

        public DeclineOrderCommandHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult<string>> Handle(DeclineOrderCommand request, CancellationToken cancellationToken)
        {
            // Check if the order exists
            var order = await _context.Orders.FindAsync(request.OrderId);

            if (order == null)
            {
                
                return Result<string>.Fail($"Order with ID  not found.");
            }

            // Check if the rider has already declined the order
            var riderOrderStatus = _context.RequestStatuses
                .FirstOrDefault(ros => ros.RiderId == request.RiderId);

            if (riderOrderStatus != null && riderOrderStatus.RiderOrderStatus == RiderOrderStatus.Declined)
            {
                // Handle the case where the rider has already declined the order
                return Result<string>.Fail($"Rider with ID  has already declined Order with ID {request.OrderId}.");
            }


            var requeststatus = new RequestStatus()
            {
                RiderId = request.RiderId,
                OrderId = request.OrderId,
                RiderOrderStatus = RiderOrderStatus.Declined,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                IsDeleted = false,
                UpdatedAt = DateTime.UtcNow,



            };

            _context.RequestStatuses.Add(requeststatus);
            await _context.SaveChangesAsync();
            return Result<string>.Success("Order successfully declined");

        }
    }
}
