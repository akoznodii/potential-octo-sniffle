using System;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Storage;

namespace Checkout.PaymentGateway.Infrastructure.Repositories
{
    public class MerchantRepository : BaseRepository<Merchant, Storage.Entities.Merchant, Guid>, IMerchantRepository
    {
        public MerchantRepository(ApplicationContext context) : base(context)
        {
        }

        protected override Storage.Entities.Merchant MapToData(Merchant domainModel)
        {
            return new Storage.Entities.Merchant
            {
                Id = domainModel.Id,
                Timestamp = domainModel.Timestamp as byte[],
                ApiKeyId = domainModel.ApiKeyId,
                PaymentStatusCallbackUrl = domainModel.PaymentStatusCallbackUrl
            };
        }

        protected override Merchant MapToDomain(Storage.Entities.Merchant dataModel)
        {
            return new Merchant
            {
                Id = dataModel.Id,
                Timestamp = dataModel.Timestamp,
                ApiKeyId = dataModel.ApiKeyId,
                PaymentStatusCallbackUrl = dataModel.PaymentStatusCallbackUrl
            };
        }
    }
}
