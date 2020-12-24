using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using Checkout.PaymentGateway.Api.Infrastructure.Authentication;
using Checkout.PaymentGateway.Api.Models;
using Checkout.PaymentGateway.Core.Incoming;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController, ApiVersion("1.0"), Route("/v{v:apiVersion}/payments")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme, Roles = ClientRoles.Merchant)]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Initiates payment processing
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessPaymentResponse))]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentRequestModel model, CancellationToken cancellationToken)
        {
            var request = new ProcessPaymentRequest
            {
                Amount = model.Amount,
                Currency = model.Currency,
                CardNumber = model.CardNumber,
                CardType = model.CardType,
                CvvNumber = model.CvvNumber,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                MerchantId = User.Identity.Name
            };

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Returns payment details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDetailsResponse))]
        public async Task<IActionResult> GetPayment(string id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetPaymentDetailsRequest { PaymentId = id }, cancellationToken);

            return Ok(response);
        }
    }
}
