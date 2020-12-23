using System.Collections.Generic;
using Checkout.PaymentGateway.Infrastructure.Storage.Entities;

namespace Checkout.PaymentGateway.Api.Infrastructure
{
    public class SeedDataSource
    {
        public IList<Merchant> Merchants { get; set; }
    }
}
