using System;
using System.Linq;
using Checkout.PaymentGateway.Infrastructure.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Api.Infrastructure
{
    public static class DatabaseExtensions
    {
        public static void EnableDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("Database:EnableMigrations"))
            {
                return;
            }

            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationContext>();
            context.Database.Migrate();
        }

        public static void SeedDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("Database:SeedData"))
            {
                return;
            }

            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationContext>();

            var data = new SeedDataSource();

            configuration.GetSection("Database:SeedDataSource").Bind(data);

            if (data.Merchants != null)
            {
                var ids = data.Merchants.Select(m => m.Id).ToList();
                var existingIds = context.Merchants.Where(m => ids.Contains(m.Id)).Select(t => t.Id).ToList();
                var range = data.Merchants.Where(m => !existingIds.Contains(m.Id)).ToList();

                if (range.Any())
                {
                    context.Merchants.AddRange(range);
                    context.SaveChanges();
                }
            }

            if (data.Clients != null)
            {
                var names = data.Clients.Select(c => c.Name).ToList();
                var existingNames = context.Clients.Where(c => names.Contains(c.Name)).Select(t => t.Name).ToList();
                var range = data.Clients.Where(c => !existingNames.Contains(c.Name)).ToList();

                if (range.Any())
                {
                    context.Clients.AddRange(range);
                    context.SaveChanges();
                }
            }
        }
    }
}
