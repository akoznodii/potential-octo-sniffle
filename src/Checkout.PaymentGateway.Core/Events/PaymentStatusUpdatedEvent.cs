using MediatR;

namespace Checkout.PaymentGateway.Core.Events
{
    public class PaymentStatusUpdatedEvent : INotification
    {
        public string PaymentId { get; set; }

        public string MerchantId { get; set; }

        public string PaymentStatus { get; set; }

        public string ProcessorTransactionId { get; set; }

        public string ProcessorStatus { get; set; }
    }
}
