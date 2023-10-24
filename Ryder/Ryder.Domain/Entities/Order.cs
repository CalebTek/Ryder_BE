using Ryder.Domain.Common;
using Ryder.Domain.Enums;

namespace Ryder.Domain.Entities
{
    public class Order : BaseEntity
    {
        
        public Address PickUpLocation { get; set; }
        public Address DropOffLocation { get; set; }
        public string PickUpPhoneNumber { get;  set; }
        public string PackageDescription { get; set; }
        public string ReferenceNumber { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid AppUserId { get; set; }
        public Guid RiderId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public RiderOrderStatus RiderOrderStatus { get; set; }


    }
}