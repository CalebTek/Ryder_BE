using Microsoft.Extensions.Configuration;
using PayStack.Net;
using Ryder.Infrastructure.Common.Extensions;
using Ryder.Infrastructure.Interface;


namespace Ryder.Infrastructure.Implementation
{
    public class PaystackService : IPaystackService
    {
        private readonly IPayStackApi _paystack;
        private readonly IConfiguration _configuration;

        public PaystackService(IPayStackApi paystack, IConfiguration configuration)
        {
            _paystack = paystack;
            _configuration = configuration;
        }

        public async Task<InitiateTransactionResponse> InitializePayment(InitiateTransactionRequest request)
        {
            var result = new InitiateTransactionResponse();
            var transactionRequest = new TransactionInitializeRequest()
            {
                AmountInKobo = request.AmountInKobo * 100,
                Email = request.Email,
                Reference = request.Reference,
                CallbackUrl = $"{_configuration["AppSettings:AppUrl"]}/verify-payment",
                Currency = request.Currency,
            };

            var response = _paystack.Transactions.Initialize(transactionRequest);

            if (response.Status)
            {
                result.Message = "Payment Initialization successful";
                result.AuthUrl = response.Data.AuthorizationUrl;
                result.Status = response.Status;
            }
            else
            {
                result.Message = "Unable to generate payment link, please try again";
            }

            return await Task.Run(() => result);
        }

        public async Task<TransactionVerifyResponse> VerifyPayment(string reference)
        {
            return await Task.Run(() => _paystack.Transactions.Verify(reference));
        }

    }
}
