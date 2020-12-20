using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Ports;
using MediatR;

namespace Checkout.PaymentGateway.Core.Handlers
{
    public class GetPaymentDetailsRequestHandler : IRequestHandler<GetPaymentDetailsRequest, PaymentDetailsResponse>
    {
        private IPaymentRepository _paymentRepository;

        public GetPaymentDetailsRequestHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }

        public async Task<PaymentDetailsResponse> Handle(GetPaymentDetailsRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var payment = await _paymentRepository.GetById(Guid.Parse(request.PaymentId));

            if (payment == null) throw new InvalidOperationException("Payment not found.");

            return new PaymentDetailsResponse
            {
                Id = request.PaymentId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status.ToString("G"),
                MerchantId = payment.MerchantId.ToString(),
                CreditCardNumber = payment.CreditCard.MaskedCardNumber,
                CreditCardExpiryDate = payment.CreditCard.ExpiryDate,
                ProcessorTransactionId = payment.Transaction?.Id,
                ProcessorTransactionStatus = payment.Transaction?.Status,
            };
        }
    }
}
