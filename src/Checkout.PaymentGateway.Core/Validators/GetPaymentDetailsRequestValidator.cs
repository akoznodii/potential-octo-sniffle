using System;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Ports;
using FluentValidation;

namespace Checkout.PaymentGateway.Core.Validators
{
    public class GetPaymentDetailsRequestValidator : AbstractValidator<GetPaymentDetailsRequest>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentDetailsRequestValidator(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));

            RuleFor(r => r.PaymentId)
                .NotEmpty()
                .Must(v => Guid.TryParse(v, out _))
                .WithMessage("Invalid Payment Id.");

            RuleFor(r => r.PaymentId)
                .MustAsync(async (id, _) => (await _paymentRepository.GetById(Guid.Parse(id))) != null)
                .WithMessage("Payment not found.")
                .When(r => Guid.TryParse(r.PaymentId, out _));
        }
    }
}
