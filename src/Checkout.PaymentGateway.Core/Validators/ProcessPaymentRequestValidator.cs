using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Options;
using Checkout.PaymentGateway.Core.Ports;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.Core.Validators
{
    public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
    {
        private static readonly List<string> CardTypesWith16DigitsNumber = new List<string>
            { CreditCardTypes.Mastercard, CreditCardTypes.Visa};

        private static readonly List<string> CardTypesWith3DigitsCvvNumber = new List<string>
            { CreditCardTypes.Mastercard, CreditCardTypes.Visa};

        private readonly IOptionsSnapshot<PaymentOptions> _options;
        private readonly IMerchantRepository _merchantRepository;

        public ProcessPaymentRequestValidator(IOptionsSnapshot<PaymentOptions> options, IMerchantRepository merchantRepository)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _merchantRepository = merchantRepository ?? throw new ArgumentNullException(nameof(merchantRepository));

            var paymentOptions = _options.Value;

            RuleFor(r => r.Amount)
                .InclusiveBetween(paymentOptions.MinAmount, paymentOptions.MaxAmount)
                .ScalePrecision(2 ,Math.Floor(paymentOptions.MaxAmount).ToString(CultureInfo.InvariantCulture).Length + 1);

            RuleFor(r => r.CvvNumber)
                .NotEmpty()
                .Matches(@"^\d{3}$")
                .WithMessage("Invalid CVV number.")
                .When(r => !string.IsNullOrEmpty(r.CardType) && CardTypesWith3DigitsCvvNumber.Any(t =>
                    string.Equals(t, r.CardType, StringComparison.InvariantCultureIgnoreCase)));

            RuleFor(r => r.Currency)
                .NotEmpty()
                .Must(v => paymentOptions.CurrencyCodes.Any(c => string.Equals(c, v, StringComparison.InvariantCultureIgnoreCase)))
                .WithMessage("Currency {PropertyValue} is not supported.");


            RuleFor(r => r.CardType)
                .NotEmpty()
                .Must(v => paymentOptions.CardTypes.Any(c => string.Equals(c, v, StringComparison.InvariantCultureIgnoreCase)))
                .WithMessage("Credit cart type {PropertyValue} is not supported.");

            RuleFor(r => r.MerchantId)
                .NotEmpty()
                .Must(v => Guid.TryParse(v, out _))
                .WithMessage("Invalid Merchant Id.");

            RuleFor(r => r.MerchantId)
                .MustAsync(async (id, _) => (await _merchantRepository.GetById(Guid.Parse(id))) != null)
                .WithMessage("Merchant not found.")
                .When(r => Guid.TryParse(r.MerchantId, out _));

            RuleFor(r => r.ExpiryMonth).InclusiveBetween(1, 12);

            RuleFor(r => r.ExpiryYear)
                .InclusiveBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year);

            RuleFor(r => r.ExpiryMonth)
                .Must((r, _) =>
                {
                    var expiryDay = DateTime.DaysInMonth(r.ExpiryYear, r.ExpiryMonth);

                    var expiryDate = new DateTime(r.ExpiryYear, r.ExpiryMonth, expiryDay);

                    return expiryDate >= DateTime.Today;
                })
                .OverridePropertyName("ExpiryDate")
                .WithMessage("Credit card is expired.")
                .When(
                    (r, _) =>
                        r.ExpiryMonth >= 1 &&
                        r.ExpiryMonth <= 12 &&
                        r.ExpiryYear >= DateTime.MinValue.Year &&
                        r.ExpiryYear <= DateTime.MaxValue.Year);

            RuleFor(r => r.CardNumber)
                .NotEmpty();

            RuleFor(r => r.CardNumber)
                .Matches(@"^\d{16}$")
                .When(r => !string.IsNullOrEmpty(r.CardType) && CardTypesWith16DigitsNumber.Any(t =>
                    string.Equals(t, r.CardType, StringComparison.InvariantCultureIgnoreCase)))
                .WithMessage("Invalid credit card number format: 16-digits number is expected.");

            RuleFor(r => r.CardNumber)
                .CreditCard()
                .When(r => long.TryParse(r.CardNumber, out _))
                .WithMessage("Invalid credit card number.");
        }
    }
}
