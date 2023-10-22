using Microsoft.AspNetCore.Mvc;
using Ryder.Application.Payment.Command;
using Ryder.Application.Payment.Query;

namespace Ryder.Api.Controllers
{
    [Route("api/payment")]
    public class PaymentController : ApiController
    {
        [HttpPost("initialize-payment")]
        public async Task<IActionResult> InitializePayment([FromBody] PaymentCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentQuery query)
        {
            return await Initiate(() => Mediator.Send(query));
        }
    }
}
