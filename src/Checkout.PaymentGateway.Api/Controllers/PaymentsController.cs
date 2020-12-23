using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Incoming;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController, ApiVersion("1.0"), Route("/v{v:apiVersion}/payments")]
    [Consumes("application/json"), Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessPaymentResponse))]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDetailsResponse))]
        public async Task<IActionResult> GetPayment(string id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetPaymentDetailsRequest { PaymentId = id }, cancellationToken);

            return Ok(response);
        }
    }
}
