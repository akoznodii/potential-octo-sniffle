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
                DisplayName = domainModel.DisplayName,
                Timestamp = domainModel.Timestamp as byte[],
                CallbackApiUrl = domainModel.CallbackApiUrl,
                CallbackApiPassword = domainModel.CallbackApiPassword,
                CallbackApiUsername = domainModel.CallbackApiUsername,
            };
        }

        protected override Merchant MapToDomain(Storage.Entities.Merchant dataModel)
        {
            return new Merchant
            {
                Id = dataModel.Id,
                DisplayName = dataModel.DisplayName,
                Timestamp = dataModel.Timestamp,
                CallbackApiUrl = dataModel.CallbackApiUrl,
                CallbackApiPassword = dataModel.CallbackApiPassword,
                CallbackApiUsername = dataModel.CallbackApiUsername,
            };
        }
    }
}
