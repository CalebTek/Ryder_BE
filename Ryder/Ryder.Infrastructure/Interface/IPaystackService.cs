using PayStack.Net;
using Ryder.Infrastructure.Common.Extensions;

namespace Ryder.Infrastructure.Interface
{
    public interface IPaystackService
    {
       Task<InitiateTransactionResponse> InitializePayment(InitiateTransactionRequest request);
        Task<TransactionVerifyResponse> VerifyPayment(string reference);
    }
}
