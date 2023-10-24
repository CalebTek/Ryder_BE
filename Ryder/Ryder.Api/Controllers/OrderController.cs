using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ryder.Application.Order.Command.AcceptOrder;
using Ryder.Application.Order.Command.EndRide;
using Ryder.Application.Order.Command.PlaceOrder;
using Ryder.Application.Order.Query.GetAllOrder;
using Ryder.Application.Order.Query.GetOderById;
using Ryder.Application.Order.Query.OrderProgress;
using MediatR;
using AspNetCoreHero.Results;
using Ryder.Domain.Context;
using Ryder.Application.Order.Query.GetAllOrderStatus;
using Ryder.Application.Rider.Query.GetRiderAvailability;
using Ryder.Application.Order.Query.GetAll;
using Ryder.Domain.Entities;

namespace Ryder.Api.Controllers
{
    public class OrderController : ApiController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
            _logger.LogInformation("OrderController initialized.");
        }


        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }


        [HttpPost("accept")]
        public async Task<IActionResult> AcceptOrder([FromBody] AcceptOrderCommand command)
        {
            _logger.LogInformation("AcceptOrder action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }


        [HttpGet("getAllOrder")]
        public async Task<IActionResult> GetAllOrder([FromQuery] Guid appUserId)
        {
            return await Initiate(() => Mediator.Send(new GetAllOrderQuery { AppUserId = appUserId }));
        }


        [HttpGet("progress")]
        public async Task<IActionResult> RequestProgress([FromBody] GetAllOrderProgressQuery query)
        {
            _logger.LogInformation("RequestProgress action invoked.");
            return await Initiate(() => Mediator.Send(query));
        }


        [HttpGet("allOrderProgress/{id}")]
        public async Task<IActionResult> AllOrderProgress(Guid id)
        {
            return await Initiate(() => Mediator.Send(new GetAllOrderStatusQuery { AppUserId = id }));
        }




        [HttpGet("{appUserId}/{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid appUserId, Guid orderId)
        {
            return await Initiate(() => Mediator.Send(new GetOrderByIdQuery { AppUserId = appUserId, OrderId = orderId }));
        }


        [HttpPost("end")]
        public async Task<IActionResult> EndRide([FromBody] EndRideCommand command)
        {
            _logger.LogInformation("EndRide action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }


        [HttpPost("decline")]
        public async Task<IActionResult> DeclineOrder([FromBody] DeclineOrderCommand command)
        {
            _logger.LogInformation("DeclineOrder action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredOrders()
        {
            _logger.LogInformation("Filtered Order action invoked");
            return await Initiate(() => Mediator.Send(new GetAllQuery()));
        }
    }
}