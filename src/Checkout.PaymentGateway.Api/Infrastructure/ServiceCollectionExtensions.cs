using System.Reflection;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Options;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Notifier;
using Checkout.PaymentGateway.Infrastructure.Processor;
using Checkout.PaymentGateway.Infrastructure.Repositories;
using Checkout.PaymentGateway.Infrastructure.Storage;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentGateway(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IPaymentProcessor, FakePaymentProcessor>()
                .AddScoped<IMerchantNotifier, MerchantNotifier>()
                .AddScoped<IMerchantRepository, MerchantRepository>()
                .AddScoped<IPaymentRepository, PaymentRepository>()
                .AddMediatR(typeof(ProcessPaymentRequestHandler))
                .AddFluentValidation(new[] {typeof(ProcessPaymentRequestHandler).GetTypeInfo().Assembly})
                .AddDbContext<ApplicationContext>(options =>
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsHistoryTable("__EFMigrationsHistory",
                            ApplicationContext.DefaultSchema));
                })
                .Configure<PaymentProcessorOptions>(configuration.GetSection("Processor"))
                .Configure<PaymentOptions>(configuration.GetSection("Payment"));
        }
    }
}
