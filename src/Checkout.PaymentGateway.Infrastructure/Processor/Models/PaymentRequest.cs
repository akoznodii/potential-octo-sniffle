namespace Checkout.PaymentGateway.Infrastructure.Processor.Models
{
    public class PaymentRequest
    {
        public string ExternalTransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string CvvNumber { get; set; }
    }
}
