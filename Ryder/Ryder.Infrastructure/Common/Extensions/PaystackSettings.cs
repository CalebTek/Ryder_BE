using Newtonsoft.Json;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Infrastructure.Common.Extensions
{
    public class PaystackSettings
    {
        public string TestSecretKey { get; set; }
    }

    public class InitiateTransactionResponse
    {
        public bool Status { get; set; }
        public string AuthUrl { get; set; }
        public string Message { get; set; }
    }

    public class InitiateTransactionRequest : RequestMetadataExtender
    {
        public string Reference { get; set; }
        public int AmountInKobo { get; set; }
        public string Email { get; set; }
        public string CallbackUrl { get; set; } 
        public string Currency { get; set; } = "NGN";
    }
}
