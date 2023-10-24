using Ryder.Domain.Common;
using System.ComponentModel;

namespace Ryder.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("Order Placed")]
        OrderPlaced = 1,

        [Description("PendingConfirmation")]
        PendingConfirmation = 2,

        [Description("Accepted")]
        Accepted = 3,

        [Description("InProgress")]
        InProgress = 4,

        [Description(" Delivered")]
        Delivered = 5,

}
}