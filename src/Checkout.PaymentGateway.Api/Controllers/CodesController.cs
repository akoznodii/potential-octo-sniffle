using System.Collections.Generic;
using AspNetCore.Authentication.ApiKey;
using Checkout.PaymentGateway.Api.Infrastructure.Authentication;
using Checkout.PaymentGateway.Api.Models;
using Checkout.PaymentGateway.Core.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController, ApiVersion("1.0"), Route("/v{v:apiVersion}/codes")]
    [Consumes("application/json"), Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme, Roles = ClientRoles.Merchant)]
    public class CodesController : ControllerBase
    {
        private readonly IOptionsSnapshot<PaymentOptions> _options;

        public CodesController(IOptionsSnapshot<PaymentOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Returns list of supported credit card types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("creditCards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public IActionResult CreditCards()
        {
            return Ok(_options.Value.CardTypes);
        }

        /// <summary>
        /// Returns list of supported currency codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("currencies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public IActionResult Currencies()
        {
            return Ok(_options.Value.CurrencyCodes);
        }

        /// <summary>
        /// Returns min and max amount of a payment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("amounts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AmountModel))]
        public IActionResult Amounts()
        {
            return Ok(new AmountModel
            {
                MinAmount = _options.Value.MinAmount,
                MaxAmount = _options.Value.MaxAmount
            });
        }
    }
}
