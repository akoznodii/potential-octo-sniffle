﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Entities
{
    public class Merchant : IEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string PaymentStatusCallbackUrl { get; set; }

        public string ApiKeyId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}