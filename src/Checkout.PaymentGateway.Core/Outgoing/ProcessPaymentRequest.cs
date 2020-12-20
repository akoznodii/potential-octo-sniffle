using System;

namespace Checkout.PaymentGateway.Core.Outgoing
{
    public class ProcessPaymentRequest
    {
        public Guid PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string CvvNumber { get; set; }
    }
}
