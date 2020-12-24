using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Entities
{
    [Table("Clients", Schema = ApplicationContext.DefaultSchema)]
    public class Client
    {
        [Key]
        public string Name { get; set; }

        public string Key { get; set; }

        public string Role { get; set; }
    }
}
