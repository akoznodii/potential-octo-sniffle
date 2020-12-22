namespace Checkout.PaymentGateway.Core.Incoming
{
    public class PaymentDetailsResponse
    {
        public string Id { get; set; }

        public string MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string ProcessorTransactionId { get; set; }

        public string ProcessorTransactionStatus { get; set; }

        public string CreditCardType { get; set; }

        public string CreditCardNumber { get; set; }

        public string CreditCardExpiryDate { get; set; }

        public string Status { get; set; }
    }
}
