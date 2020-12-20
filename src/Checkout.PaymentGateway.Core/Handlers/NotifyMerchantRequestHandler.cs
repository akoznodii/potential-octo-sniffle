using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Ports;
using MediatR;

namespace Checkout.PaymentGateway.Core.Handlers
{
    public class NotifyMerchantRequestHandler : IRequestHandler<NotifyMerchantRequest>
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMerchantNotifier _merchantNotifier;

        public NotifyMerchantRequestHandler(IMerchantRepository merchantRepository, IMerchantNotifier merchantNotifier)
        {
            _merchantRepository = merchantRepository ?? throw new ArgumentNullException(nameof(merchantRepository));
            _merchantNotifier = merchantNotifier ?? throw new ArgumentNullException(nameof(merchantNotifier));
        }

        public async Task<Unit> Handle(NotifyMerchantRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var merchant = await _merchantRepository.GetById(Guid.Parse(request.MerchantId));

            if (merchant == null) throw new InvalidOperationException("Merchant not found.");

            if (merchant.PaymentStatusCallbackUrl != null)
            {
                var notifyRequest = new Outgoing.NotifyMerchantRequest
                {
                    CallbackUrl = merchant.PaymentStatusCallbackUrl,
                    PaymentId = request.PaymentId,
                    PaymentStatus = request.PaymentStatus,
                    ProcessorTransactionId = request.ProcessorTransactionId
                };

                await _merchantNotifier.Notify(notifyRequest, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
