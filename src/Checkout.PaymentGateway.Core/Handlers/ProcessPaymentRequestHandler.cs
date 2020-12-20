using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using MediatR;

namespace Checkout.PaymentGateway.Core.Handlers
{
    public class ProcessPaymentRequestHandler : IRequestHandler<ProcessPaymentRequest, ProcessPaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentProcessor _paymentProcessor;

        public ProcessPaymentRequestHandler(IPaymentRepository paymentRepository, IPaymentProcessor paymentProcessor)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
        }

        public async Task<ProcessPaymentResponse> Handle(ProcessPaymentRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var payment = new Payment
            {
                MerchantId = request.MerchantId,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Initial,
                CreditCard = new CreditCard
                {
                    CardNumber = request.CardNumber,
                    ExpiryMonth = request.ExpiryMonth,
                    ExpiryYear = request.ExpiryYear
                }
            };

            await _paymentRepository.Add(payment);

            var processPaymentRequest = new Outgoing.ProcessPaymentRequest
            {
                PaymentId = payment.Id,
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.CardNumber,
                CvvNumber = request.CvvNumber,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear
            };

            var processPaymentResponse = await _paymentProcessor.Process(processPaymentRequest, cancellationToken);

            payment.Status = processPaymentResponse.MappedStatus;

            payment.Transaction = new PaymentProcessorTransaction
            {
                Id = processPaymentResponse.TransactionId,
                Status = processPaymentResponse.RawStatus
            };

            await _paymentRepository.Update(payment);

            return new ProcessPaymentResponse
            {
                PaymentId = payment.Id
            };
        }
    }
}
