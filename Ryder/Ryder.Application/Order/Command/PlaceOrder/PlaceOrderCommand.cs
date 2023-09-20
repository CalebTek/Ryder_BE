using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Command.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<IResult<Guid>>
    {
        public PlaceOrderAddressCommand PickUpLocation { get; set; }
        public PlaceOrderAddressCommand DropOffLocation { get; set; }
        public string PickUpPhoneNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public Guid RiderId { get; set; }
    }

    public class PlaceOrderAddressCommand
    {
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
    }
}