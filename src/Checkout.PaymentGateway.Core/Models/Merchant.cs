﻿using System;

namespace Checkout.PaymentGateway.Core.Models
{
    public class Merchant : IEntity<Guid>, ITrackable
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string CallbackApiUrl { get; set; }

        public string CallbackApiUsername { get; set; }

        public string CallbackApiPassword { get; set; }

        public object Timestamp { get; set; }
    }
}
