using System;

namespace Checkout.PaymentGateway.Core.Models
{
    public class Payment : IEntity<Guid>, ITrackable
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public PaymentProcessorTransaction Transaction { get; set; }

        public CreditCard CreditCard { get; set; }

        public PaymentStatus Status { get; set; }

        public object Timestamp { get; set; }
    }
}
