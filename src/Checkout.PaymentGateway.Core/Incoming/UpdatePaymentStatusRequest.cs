using MediatR;

namespace Checkout.PaymentGateway.Core.Incoming
{
    public class UpdatePaymentStatusRequest : IRequest
    {
        public string TransactionId { get; set; }

        public string RawStatus { get; set; }
    }
}
