using AspNetCoreHero.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using Ryder.Application.Transfer.Command;
using Serilog;

namespace Ryder.Application.Transfer.Query
{
    public class TransferQueryHandler : IRequestHandler<TransferQuery, IResult<TransferQueryResponse>>
    {
        private readonly IConfiguration _configuration;

        public TransferQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IResult<TransferQueryResponse>> Handle(TransferQuery request, CancellationToken cancellationToken)
        {
            var response = new TransferQueryResponse();
            string finalizeTransferUrl = _configuration["PaystackTransfer:FinalizeTransferUrl"];
            string PayStackApiKey = "Bearer " + _configuration["Paystack:TestSecretKey"];

            try
            {
                var client = new RestClient(finalizeTransferUrl);
                var finalizeRequest = new RestRequest(resource: "", method: Method.Post);

                finalizeRequest.AddHeader("Authorization", PayStackApiKey);
                finalizeRequest.AddHeader("Content-Type", "application/json");

                // Create the JSON data for finalizing the transfer
                string jsonData = $"{{ \"transfer_code\": \"{request.TransferCode}\", \"otp\": \"{request.Otp}\" }}";
                finalizeRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);

                var clientResponse = client.Execute(finalizeRequest);
                if(clientResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Finalize Transfer was successful
                    var responseContent = clientResponse.Content; // Get the response content as a string

                    var jObject = JObject.Parse(responseContent);

                    response.Status = jObject["status"].Value<bool>();
                    response.Message = jObject["message"].Value<string>();
                    var data = jObject["data"];
                    if (data != null)
                    {
                        response.Amount = data["amount"].Value<int>();
                        response.Currency = data["currency"].Value<string>();
                        response.Reference = data["reference"].Value<string>();
                        response.ReasonForTransfer = data["reason"].Value<string>();
                        response.TransferCode = data["transfer_code"].Value<string>();
                        response.RiderId = request.RiderId;
                    }

                }
                else
                {
                    // Finalizing Transfer Failed
                    response.Status = false;
                    response.Message = $"Transfer initialization failed with status code: {clientResponse.StatusCode}";
                }
                return Result<TransferQueryResponse>.Success(response);

            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");

                // Handle the exception and provide an error message
                response.Status = false;
                response.Message = "An error occurred while Finalizing the transfer: " + ex.Message;
                return Result<TransferQueryResponse>.Fail(response.Message);
            }
           
        }
    }
}
