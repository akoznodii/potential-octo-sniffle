using System;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Ports
{
    public interface IMerchantRepository : IRepository<Merchant, Guid>
    {
    }
}
