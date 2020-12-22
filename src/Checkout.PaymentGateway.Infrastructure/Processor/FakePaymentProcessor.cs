﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Outgoing;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Processor.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace Checkout.PaymentGateway.Infrastructure.Processor
{
    public class FakePaymentProcessor : IPaymentProcessor
    {
        private readonly IOptionsSnapshot<FakePaymentProcessorOptions> _options;

        public FakePaymentProcessor(IOptionsSnapshot<FakePaymentProcessorOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ProcessPaymentResponse> Process(ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            var options = _options.Value;

            var client = new RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(options.Username, options.Password, Encoding.UTF8)
            };

            // Assume that Fake Payment Processor has its own API and Contracts provided for use
            var outgoingRequest = new JsonRequest<PaymentRequest, PaymentResponse>(options.Url, new PaymentRequest
            {
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.CardNumber,
                CardType = request.CardType,
                CvvNumber = request.CvvNumber,
                ExpiryDate = $"{request.ExpiryMonth}/{request.ExpiryYear % 100}",
                ExternalTransactionId = request.PaymentId.ToString("G")
            });

            var outgoingResponse = await client.PostAsync<PaymentResponse>(outgoingRequest, cancellationToken);

            var status = MapStatus(outgoingResponse.Status);

            return new ProcessPaymentResponse
            {
                MappedStatus = status,
                RawStatus = outgoingResponse.Status,
                TransactionId = outgoingResponse.Id
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
