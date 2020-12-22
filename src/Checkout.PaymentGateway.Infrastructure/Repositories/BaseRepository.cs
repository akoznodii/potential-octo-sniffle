using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Infrastructure.Storage;
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
        }

        public async Task Update(TDomainModel entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var data = MapToData(entity);

            Context.Set<TDataModel>().Update(data);
            await Context.SaveChangesAsync();
        }

        public async Task<TDomainModel> GetById(TId id)
        {
            var data = await Context.Set<TDataModel>().AsNoTracking().FirstOrDefaultAsync(d => d.Id.Equals(id));

            return MapToDomain(data);
        }

        protected abstract TDataModel MapToData(TDomainModel domainModel);

        protected abstract TDomainModel MapToDomain(TDataModel dataModel);
    }
}
