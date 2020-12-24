namespace Checkout.PaymentProcessorMock.Api.Models
{
    public class NotificationRequest
    {
        public string TransactionId { get; set; }

        public string TransactionStatus { get; set; }
    }
}
