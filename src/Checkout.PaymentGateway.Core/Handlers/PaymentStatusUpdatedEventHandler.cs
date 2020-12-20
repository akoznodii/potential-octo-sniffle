using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Events;
using Checkout.PaymentGateway.Core.Incoming;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Core.Handlers
{
    public class PaymentStatusUpdatedEventHandler : INotificationHandler<PaymentStatusUpdatedEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentStatusUpdatedEventHandler> _logger;

        public PaymentStatusUpdatedEventHandler(IMediator mediator, ILogger<PaymentStatusUpdatedEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(PaymentStatusUpdatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var request = new NotifyMerchantRequest
            {
                MerchantId = notification.MerchantId,
                PaymentId = notification.PaymentId,
                PaymentStatus = notification.PaymentStatus,
                ProcessorStatus = notification.ProcessorStatus,
                ProcessorTransactionId = notification.ProcessorTransactionId
            };

            // Fire and forget
            _mediator.Send(request, cancellationToken)
                .ContinueWith(t => { _logger.LogError(t.Exception, "Failed to notify merchant."); },
                    TaskContinuationOptions.OnlyOnFaulted);

            return Task.CompletedTask;
        }
    }
}
