using AspNetCoreHero.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;


namespace Ryder.Application.Transfer.Command
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, IResult<TransferCommandResponse>>
    {
        private readonly IConfiguration _configuration;
        public TransferCommandHandler(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<IResult<TransferCommandResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var response = new TransferCommandResponse();
           string initialTransferUrl = _configuration["PaystackTransfer:InitialTransferUrl"];
            string PayStackApiKey = "Bearer " + _configuration["Paystack:TestSecretKey"];

            try
            {
                var client = new RestClient(initialTransferUrl);
                var transferRequest = new RestRequest(resource: "", method: Method.Post);

                transferRequest.AddHeader("Authorization", PayStackApiKey);
                transferRequest.AddHeader("Content-Type", "application/json");

                // Create the JSON data for the transfer
                string jsonData = $"{{ \"source\": \"balance\", \"reason\": \"{request.ReasonForTransfer}\", \"amount\": {request.Amount}, \"recipient\": \"{request.Recipient}\" }}";
                transferRequest.AddParameter("application/json", jsonData, ParameterType.RequestBody);

                var clientResponse = client.Execute(transferRequest);
                if (clientResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    // Transfer Initialization was successful
                    var responseContent = clientResponse.Content; // Get the response content as a string

                    // Use a JSON library or method to extract specific properties
                    response.Status = JObject.Parse(responseContent)["status"].Value<bool>();
                    response.Message = JObject.Parse(responseContent)["message"].Value<string>();
                    response.Amount = JObject.Parse(responseContent)["data"]["amount"].Value<int>();
                    response.Currency = JObject.Parse(responseContent)["data"]["currency"].Value<string>();
                    response.TransferCode = JObject.Parse(responseContent)["data"]["transfer_code"].Value<string>();
                    response.ReasonForTransfer = JObject.Parse(responseContent)["data"]["reason"].Value<string>();
                    response.Recipient = JObject.Parse(responseContent)["data"]["recipient"].Value<string>();
                    response.RiderId = request.RiderId;
                }
                else
                {
                    // Transfer Initialization failed
                    response.Status = false;
                    response.Message = $"Transfer initialization failed with status code: {clientResponse.StatusCode}";
                }
                return Result<TransferCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log details, and return an appropriate error result.
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");

                // Handle the exception and provide an error message
                response.Status = false;
                response.Message = "An error occurred while initializing the transfer: " + ex.Message;
                return Result<TransferCommandResponse>.Fail(response.Message);
            }

        }
    }
}
