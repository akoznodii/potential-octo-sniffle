using MediatR;

namespace Checkout.PaymentGateway.Core.Incoming
{
    public class GetPaymentDetailsRequest : IRequest<PaymentDetailsResponse>
    {
        public string PaymentId { get; set; }
    }
}
