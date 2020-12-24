using Checkout.PaymentGateway.Infrastructure.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.Storage
{
    public class ApplicationContext : DbContext
    {
        public const string DefaultSchema = "gateway";

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
