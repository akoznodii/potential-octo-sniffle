using System;
using MediatR;

namespace Checkout.PaymentGateway.Core.Incoming
{
    public class ProcessPaymentRequest : IRequest<ProcessPaymentResponse>
    {
        public Guid MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string CvvNumber { get; set; }
    }
}
