using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Ports
{
    public interface IRepository<TEntity, in TId>
        where TEntity : IEntity<TId>
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task<TEntity> GetById(TId id);
    }
}
