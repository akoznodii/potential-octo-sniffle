using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Entities
{
    [Table("Merchants", Schema = ApplicationContext.DefaultSchema)]
    public class Merchant : IEntity<Guid>, IWithTimestamp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string CallbackApiUrl { get; set; }

        public string CallbackApiUsername { get; set; }

        public string CallbackApiPassword { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
