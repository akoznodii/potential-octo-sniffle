namespace Checkout.PaymentGateway.Api.Models
{
    public class ProcessPaymentRequestModel
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string CvvNumber { get; set; }
    }
}
