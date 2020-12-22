using Checkout.PaymentGateway.Infrastructure.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.Storage
{
    public class ApplicationContext : DbContext
    {
        public static string DefaultSchema { get; set; } = "gateway";

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Merchant> Merchants { get; set; }
    }
}
