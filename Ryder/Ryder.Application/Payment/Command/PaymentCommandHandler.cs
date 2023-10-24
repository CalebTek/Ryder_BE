using AspNetCoreHero.Results;
using MediatR;
using PayStack.Net;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Utility;
using Serilog;

namespace Ryder.Application.Payment.Command
{
    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, IResult<PaymentResponse>>
    {
        private readonly IPayStackApi _paystack;
        private readonly ISmtpEmailService _emailService;

        public PaymentCommandHandler(IPayStackApi paystack, ISmtpEmailService emailService)
        {
            _paystack = paystack;
            _emailService = emailService;
        }
        public async Task<IResult<PaymentResponse>> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var response = new PaymentResponse();

            try
            {
                // Create a new TransactionInitializeRequest
                var transactionRequest = new TransactionInitializeRequest
                {
                    AmountInKobo = request.AmountInKobo * 100,
                    Email = request.Email,
                    Reference = PaystackUtility.GenerateUniqueReference(),
                    Currency = request.Currency,
                    CallbackUrl = request.CallbackUrl
                };

                // Initialize the payment using PayStack API
                var paymentInitiationResponse = _paystack.Transactions.Initialize(transactionRequest);


                if (paymentInitiationResponse.Status)
                {
                    // Payment initialization was successful
                    response.Status = true;
                    response.AuthUrl = paymentInitiationResponse.Data.AuthorizationUrl;
                    response.Message = "Payment Initialization successful";

                    // Email subject
                    var emailSubject = "Payment Confirmation - Please Complete Your Payment";

                    // Email message
                    var emailMessage = $@"
                        Thank you for choosing Ryder! You're just one step away from completing your payment for your recent order. 
                        To make the payment process as smooth as possible, please click the link below to proceed with the payment:

                        {response.AuthUrl}

                        We appreciate your business and look forward to serving you again in the future.

                        Best regards,
                        The Ryder Team
                        ";

                    // send the email
                    var emailSent = await _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);
                }
                else
                {
                    // Payment initialization failed
                    response.Status = false;
                    response.Message = "Unable to generate payment link, please try again";
                }

                return Result<PaymentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");

                // Handle the exception and provide an error message
                response.Status = false;
                response.Message = "An error occurred while initializing the payment: " + ex.Message;
                return Result<PaymentResponse>.Fail(response.Message);
            }
        }
    }
}
