using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Outgoing;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Processor.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace Checkout.PaymentGateway.Infrastructure.Processor
{
    public class MockedPaymentProcessor : IPaymentProcessor
    {
        private readonly ILogger<MockedPaymentProcessor> _logger;
        private readonly IOptionsSnapshot<PaymentProcessorOptions> _options;

        public MockedPaymentProcessor(IOptionsSnapshot<PaymentProcessorOptions> options, ILogger<MockedPaymentProcessor> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProcessPaymentResponse> Process(ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            var options = _options.Value;

            var client = new RestClient();

            // Assume that Fake Payment Processor has its own API and Contracts provided for use
            var outgoingRequest = new JsonRequest<PaymentRequest, PaymentResponse>(options.Url, new PaymentRequest
            {
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.CardNumber,
                CardType = request.CardType,
                CvvNumber = request.CvvNumber,
                ExpiryDate = $"{request.ExpiryMonth}/{request.ExpiryYear % 100}",
                ExternalTransactionId = request.PaymentId.ToString("N")
            });

            outgoingRequest.AddHeader("x-fake-pp-api-key", options.ApiKey);

            var status = PaymentStatus.Initial;
            PaymentResponse outgoingResponse = null;

            try
            {
                outgoingResponse = await client.PostAsync<PaymentResponse>(outgoingRequest, cancellationToken);
                status = MapStatus(outgoingResponse.Status);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to forward a payment process request {PaymentId}", request.PaymentId);
                status = PaymentStatus.Failed;
            }

            return new ProcessPaymentResponse
            {
                MappedStatus = status,
                RawStatus = outgoingResponse?.Status,
                TransactionId = outgoingResponse?.Id
            };
        }

        public PaymentStatus MapStatus(string status)
        {
            return status switch
            {
                "ACCEPTED" => PaymentStatus.Pending,
                "APPROVED" => PaymentStatus.Approved,
                "FRAUD" => PaymentStatus.Declined,
                "DECLINED" => PaymentStatus.Declined,
                "INVALID_DATA" => PaymentStatus.Declined,
                _ => PaymentStatus.Failed
            };
        }
    }
}
