using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;

namespace Ryder.Application.Order.Command.EndRide
{
    public class EndRideResponse
    {
        public Guid OrderId { get; set; }
        public Address DropOffLocation { get; set; }
        public Guid RiderId { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
        public string PickUpPhoneNumber { get; set; }
        
    }


}
