using AspNetCoreHero.Results;
using MediatR;
using PayStack.Net;
using Serilog;

namespace Ryder.Application.Payment.Query
{
    internal class VerifyPaymentQueryHandler : IRequestHandler<VerifyPaymentQuery, IResult<VerifyPaymentResponse>>
    {
        private readonly IPayStackApi _paystack;

        public VerifyPaymentQueryHandler(IPayStackApi paystack)
        {
            _paystack = paystack;
        }

        public async Task<IResult<VerifyPaymentResponse>> Handle(VerifyPaymentQuery request, CancellationToken cancellationToken)
        {
            var response = new VerifyPaymentResponse();

            try
            {
                // Use the PayStack API to verify the payment using the provided reference
                var verificationResponse = _paystack.Transactions.Verify(request.PaymentReference);

                if (verificationResponse.Status)
                {
                    // Payment verification was successful
                    response.IsPaymentValid = true;
                    response.Message = "Payment is valid and confirmed.";
                }
                else
                {
                    // Payment verification failed
                    response.IsPaymentValid = false;
                    response.Message = "Payment verification failed. Please check the payment reference and try again.";
                }

                return Result<VerifyPaymentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred while verifying the payment: {ex.Message}");

                // Handle the exception and provide an error message
                response.IsPaymentValid = false;
                response.Message = "An error occurred while verifying the payment: " + ex.Message;
                return Result<VerifyPaymentResponse>.Fail(response.Message);
            }
        }
    }
}
