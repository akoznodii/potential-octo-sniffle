using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Ports
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Task<Payment> GetByProcessorTransactionId(string transactionId);
    }
}
