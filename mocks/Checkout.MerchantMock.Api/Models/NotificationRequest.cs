namespace Checkout.MerchantMock.Api.Models
{
    public class NotificationRequest
    {
        public string PaymentId { get; set; }

        public string PaymentStatus { get; set; }

        public string ProcessorTransactionId { get; set; }
    }
}
