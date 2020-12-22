using System;

namespace Checkout.PaymentGateway.Core.Models
{
    public class Merchant : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string CallbackApiUrl { get; set; }

        public string CallbackApiUsername { get; set; }

        public string CallbackApiPassword { get; set; }

        public string ApiKeyId { get; set; }

        public object Timestamp { get; set; }
    }
}
