using Ryder.Domain.Common;
using Ryder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Domain.Entities
{
    public class RequestStatus : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid RiderId { get; set; }
        public RiderOrderStatus RiderOrderStatus { get; set; }
    }
}
