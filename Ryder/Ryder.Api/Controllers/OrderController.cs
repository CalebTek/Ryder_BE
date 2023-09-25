using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ryder.Application.order.Query.AcceptOrder;
using Ryder.Application.order.Query.EndRide;
using Ryder.Application.order.Query.OrderProgress;
using Ryder.Application.Order.Command.PlaceOrder;
using Ryder.Application.Order.Query.GetAllOrder;
using Ryder.Application.Order.Query.GetOderById;

namespace Ryder.Api.Controllers
{
    public class OrderController : ApiController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(MediatR.IMediator @object, ILogger<OrderController> logger)
        {
            _logger = logger;
            _logger.LogInformation("OrderController initialized.");
        }

        [AllowAnonymous]
        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand placeOrder)
        {
            return await Initiate(() => Mediator.Send(placeOrder));
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptOrder([FromBody] AcceptOrderCommand command)
        {
            _logger.LogInformation("AcceptOrder action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpGet("getAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            return await Initiate(() => Mediator.Send(new GetAllOrderQuery()));
        }

        [HttpPost("progress")]
        public async Task<IActionResult> RequestProgress([FromBody] OrderProgressCommand command)
        {
            _logger.LogInformation("RequestProgress action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            return await Initiate(() => Mediator.Send(new GetOrderByIdQuery { OrderId = orderId }));
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndRide([FromBody] EndRideCommand command)
        {
            _logger.LogInformation("EndRide action invoked.");
            return await Initiate(() => Mediator.Send(command));
        }
    }
}