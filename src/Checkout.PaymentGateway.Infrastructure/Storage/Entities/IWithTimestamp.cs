namespace Checkout.PaymentGateway.Infrastructure.Storage.Entities
{
    public interface IWithTimestamp
    {
        byte[] Timestamp { get; set; }
    }
}
