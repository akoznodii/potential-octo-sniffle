using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Storage;
using Checkout.PaymentGateway.Infrastructure.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Infrastructure.Repositories
{
    public abstract class BaseRepository<TDomainModel, TDataModel, TId> :
        IRepository<TDomainModel, TId>
        where TDomainModel : IEntity<TId>
        where TDataModel : class, IEntity<TId>
    {
        protected BaseRepository(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected ApplicationContext Context { get; }

        public async Task Add(TDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var data = MapToData(entity);

            await Context.Set<TDataModel>().AddAsync(data);
            await Context.SaveChangesAsync();

            Context.Entry(data).State = EntityState.Detached;

            entity.Id = data.Id;

            SetTimestamp(data, entity);
        }

        public async Task Update(TDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var data = MapToData(entity);

            Context.Set<TDataModel>().Update(data);
            await Context.SaveChangesAsync();

            Context.Entry(data).State = EntityState.Detached;

            SetTimestamp(data, entity);
        }

        public async Task<TDomainModel> GetById(TId id)
        {
            var data = await Context.Set<TDataModel>().AsNoTracking().FirstOrDefaultAsync(d => d.Id.Equals(id));

            return MapToDomain(data);
        }

        protected abstract TDataModel MapToData(TDomainModel domainModel);

        protected abstract TDomainModel MapToDomain(TDataModel dataModel);

        private void SetTimestamp(TDataModel data, TDomainModel entity)
        {
            if (data is IWithTimestamp dataWithTimestamp &&
                entity is ITrackable trackable)
            {
                trackable.Timestamp = dataWithTimestamp.Timestamp;
            }
        }
    }
}
