namespace Checkout.PaymentGateway.Core.Models
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
