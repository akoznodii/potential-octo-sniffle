namespace Checkout.PaymentGateway.Infrastructure.Processor.Models
{
    public class NotificationRequest
    {
        public string TransactionId { get; set; }

        public string TransactionStatus { get; set; }
    }
}
