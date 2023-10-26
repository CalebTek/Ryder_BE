using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Transfer.Command
{
    public class TransferCommandResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string TransferCode { get; set; }
        public string ReasonForTransfer { get; set; }
        public string Recipient { get; set; }
        public Guid RiderId { get; set; }
    }
}
