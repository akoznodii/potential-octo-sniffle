using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Merchant")]
        public Guid MerchantId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string ProcessorTransactionId { get; set; }

        public string ProcessorTransactionStatus { get; set; }

        public string CreditCardType { get; set; }

        public string CreditCardNumber { get; set; }

        public int CreditExpiryMonth { get; set; }

        public int CreditExpiryYear { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Merchant Merchant { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
