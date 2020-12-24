using System;
using System.Threading.Tasks;
using Checkout.PaymentProcessorMock.Api.Models;
using Checkout.PaymentProcessorMock.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Checkout.PaymentProcessorMock.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IOptionsSnapshot<PaymentGatewayOptions> _options;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger, IOptionsSnapshot<PaymentGatewayOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpPost]
        public IActionResult Process(PaymentRequest request)
        {
            _logger.LogInformation("Processing payment request: {RequestId}. It may take up to 5 seconds...",
                request.ExternalTransactionId);

            var id = Guid.NewGuid().ToString("N");

            var random = new Random(DateTime.Now.Second);

            Task.Delay(TimeSpan.FromSeconds(random.Next(1, 6))).ContinueWith(t =>
            {
                var status = GetStatus(request);

                _logger.LogInformation("Complete processing payment request: {RequestId} with {Result} result",
                    request.ExternalTransactionId, status);

                MakeCallback(id, status);
            });

            return Ok(new PaymentResponse {Id = id, Status = "ACCEPTED"});
        }

        private void MakeCallback(string id, string status)
        {
            var url = _options.Value.Url;

            _logger.LogInformation("Invoking webhook url: {Url}", url);
            var client = new RestClient();
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Accept", "application/json");

            var body = new NotificationRequest
            {
                TransactionId = id,
                TransactionStatus = status
            };

            request.AddJsonBody(body);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to invoke webhook url: {ErrorMessage}", response.ErrorMessage);
            }
        }

        private string GetStatus(PaymentRequest request)
        {
            return request.CvvNumber switch
            {
                "666" => "FRAUD",
                "403" => "DECLINED",
                "400" => "INVALID_DATA",
                _ => "APPROVED"
            };
        }
    }
}
