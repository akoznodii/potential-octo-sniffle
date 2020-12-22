using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment, Storage.Entities.Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context) : base(context)
        {
        }

        protected override Storage.Entities.Payment MapToData(Payment domainModel)
        {
            return new Storage.Entities.Payment
            {
                Amount = domainModel.Amount,
                Currency = domainModel.Currency,
                Id = domainModel.Id,
                Status = domainModel.Status,
                Timestamp = domainModel.Timestamp as byte[],
                MerchantId = domainModel.MerchantId,
                CreditCardNumber = domainModel.CreditCard.CardNumber,
                CreditCardType = domainModel.CreditCard.CardType,
                CreditCardExpiryMonth = domainModel.CreditCard.ExpiryMonth,
                CreditCardExpiryYear = domainModel.CreditCard.ExpiryYear,
                ProcessorTransactionId = domainModel.Transaction?.Id,
                ProcessorTransactionStatus = domainModel.Transaction?.Status
            };
        }

        protected override Payment MapToDomain(Storage.Entities.Payment dataModel)
        {
            var transaction = string.IsNullOrEmpty(dataModel.ProcessorTransactionId)
                ? null
                : new PaymentProcessorTransaction
                {
                    Id = dataModel.ProcessorTransactionId,
                    Status = dataModel.ProcessorTransactionStatus
                };

            var creditCard = new CreditCard
                {
                    CardNumber = dataModel.CreditCardNumber,
                    CardType = dataModel.CreditCardType,
                    ExpiryMonth = dataModel.CreditCardExpiryMonth,
                    ExpiryYear = dataModel.CreditCardExpiryYear
                };

            return new Payment
            {
                Amount = dataModel.Amount,
                Currency = dataModel.Currency,
                Id = dataModel.Id,
                Status = dataModel.Status,
                Timestamp = dataModel.Timestamp,
                Transaction = transaction,
                MerchantId = dataModel.MerchantId,
                CreditCard = creditCard
            };
        }

        public async Task<Payment> GetByProcessorTransactionId(string transactionId)
        {
            var data = await Context.Payments.AsNoTracking().FirstOrDefaultAsync(d => d.ProcessorTransactionId.Equals(transactionId));

            return MapToDomain(data);
        }
    }
}
