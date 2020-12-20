using System;

namespace Checkout.PaymentGateway.Core.Models
{
    public class Merchant : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string PaymentStatusCallbackUrl { get; set; }
    }
}
