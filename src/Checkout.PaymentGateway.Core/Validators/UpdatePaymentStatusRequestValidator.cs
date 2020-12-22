using System;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Ports;
using FluentValidation;

namespace Checkout.PaymentGateway.Core.Validators
{
    public class UpdatePaymentStatusRequestValidator : AbstractValidator<UpdatePaymentStatusRequest>
    {
        private readonly IPaymentRepository _paymentRepository;
        public UpdatePaymentStatusRequestValidator(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));

            RuleFor(r => r.RawStatus).NotEmpty();
            RuleFor(r => r.TransactionId).NotEmpty();

            RuleFor(r => r.TransactionId)
                .MustAsync(async (id, _) => (await _paymentRepository.GetByProcessorTransactionId(id)) != null)
                .WithMessage("Payment not found.")
                .When(r => !string.IsNullOrEmpty(r.TransactionId));
        }
    }
}
