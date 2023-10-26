using Microsoft.AspNetCore.Mvc;
using Ryder.Application.Transfer.Command;
using Ryder.Application.Transfer.Query;

namespace Ryder.Api.Controllers
{
  
    public class TransferController : ApiController
    {
        [HttpPost("initiate-transfer")]
        public async Task<IActionResult> InitiateTransfer([FromBody] TransferCommand command)
        {
            return await Initiate(() => Mediator.Send(command));
        }

        [HttpPost("finalize-transfer")]
        public async Task<IActionResult> FinalizeTransfer([FromBody] TransferQuery query)
        {
            return await Initiate(() => Mediator.Send(query));
        }
    }
}
