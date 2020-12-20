namespace Checkout.PaymentGateway.Core.Models
{
    public enum PaymentStatus
    {
        Initial = 0,
        Pending = 1,
        Approved = 2,
        Declined = 3,
        Failed = 4
    }
}
