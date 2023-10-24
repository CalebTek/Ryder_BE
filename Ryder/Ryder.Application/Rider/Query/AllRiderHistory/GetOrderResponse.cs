using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.AllRiderHistory
{
    public class GetOrderResponse
    {
        public Location PickUpLocation { get; set; }
        public Location DropOffLocation { get; set; }
        public string PickUpPhoneNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid RiderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
    }
}