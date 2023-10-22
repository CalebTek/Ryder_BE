using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Payment.Query
{
    public class VerifyPaymentResponse
    {
        public bool IsPaymentValid { get; set; }
        public string Message { get; set; }
    }
}
