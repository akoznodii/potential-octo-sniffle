using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Checkout.PaymentGateway.Api.Infrastructure.Authentication;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Infrastructure.Processor.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("webhooks")]
    [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme, Roles = ClientRoles.Processor)]
    public class WebhooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebhooksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Fake Payment Processor Integration: Webhook endpoint for receiving status notification of a particular payment
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("fakepp/v{v:apiVersion}/notifications")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
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
