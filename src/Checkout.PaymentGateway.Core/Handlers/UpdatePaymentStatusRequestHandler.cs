using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Events;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using MediatR;

namespace Checkout.PaymentGateway.Core.Handlers
{
    public class UpdatePaymentStatusRequestHandler : IRequestHandler<UpdatePaymentStatusRequest>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IMediator _mediator;
        public UpdatePaymentStatusRequestHandler(IPaymentRepository paymentRepository, IPaymentProcessor paymentProcessor, IMediator mediator)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(UpdatePaymentStatusRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var payment = await _paymentRepository.GetByProcessorTransactionId(request.TransactionId);

            if (payment == null) throw new InvalidOperationException("Payment not found.");

            if (payment.Status == PaymentStatus.Approved ||
                payment.Status == PaymentStatus.Declined)
            {
                return Unit.Value;
            }

            var status = _paymentProcessor.MapStatus(request.RawStatus);

            if (payment.Status != status)
            {
                payment.Status = status;
                payment.Transaction = new PaymentProcessorTransaction
                {
                    Id = request.RawStatus,
                    Status = request.RawStatus
                };

                // TODO add a retry MediatR decorator and setup EF optimistic concurrency
                await _paymentRepository.Update(payment);

                var paymentStatusUpdatedEvent = new PaymentStatusUpdatedEvent
                {
                    MerchantId = payment.MerchantId.ToString(),
                    PaymentId = payment.Id.ToString(),
                    PaymentStatus = payment.Status.ToString("G"),
                    ProcessorStatus = payment.Transaction.Status,
                    ProcessorTransactionId = payment.Transaction.Id
                };

                await _mediator.Publish(paymentStatusUpdatedEvent, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
