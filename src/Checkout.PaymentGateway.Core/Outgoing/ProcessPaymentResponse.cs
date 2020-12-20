using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Outgoing
{
    public class ProcessPaymentResponse
    {
        public string TransactionId { get; set; }
        public string RawStatus { get; set; }
        public PaymentStatus MappedStatus { get; set; }
    }
}
