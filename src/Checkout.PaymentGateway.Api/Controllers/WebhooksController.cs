using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Infrastructure.Processor.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("webhooks")]
    public class WebhooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebhooksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("fakepp/v{v:apiVersion}/notifications")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReceiveNotification(NotificationRequest notification, CancellationToken cancellationToken)
        {
            var request = new UpdatePaymentStatusRequest
            {
                RawStatus = notification.TransactionStatus,
                TransactionId = notification.TransactionId
            };

            await _mediator.Send(request, cancellationToken);

            return Ok();
        }
    }
}
