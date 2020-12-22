namespace Checkout.PaymentGateway.Core.Outgoing
{
    public class NotifyMerchantRequest
    {
        public string CallbackUrl { get; set; }

        public string CallbackApiUsername { get; set; }

        public string CallbackApiPassword { get; set; }

        public string PaymentId { get; set; }

        public string PaymentStatus { get; set; }

        public string ProcessorTransactionId { get; set; }
    }
}
