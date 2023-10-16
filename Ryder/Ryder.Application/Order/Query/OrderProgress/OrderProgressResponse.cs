using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Order.Query.OrderProgress
{
    public class OGetAllOrderrderProgressResponse
    {  
        public string Status { get; set; }
        public decimal Amount { get; set; }

        public DateTime  UpdatedAt { get; set; }


    }
}
