using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Core.Tests.Common;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Handlers
{
    public class GetPaymentDetailsRequestHandlerTests
    {
        private Mock<IPaymentRepository> _repositoryMock;

        private GetPaymentDetailsRequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IPaymentRepository>();
            _handler = new GetPaymentDetailsRequestHandler(_repositoryMock.Object);
        }

        [Test]
        public void Given_HandleMethod_When_NullRequestProvided_Should_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public void Given_HandleMethod_When_NoPaymentWithSpecifiedId_Should_ThrowException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(Fakers.GetPaymentDetailsRequest.Generate(), CancellationToken.None));
        }

        [Test]
        public async Task Given_HandleMethod_When_ValidRequestProvided_Should_ReturnResponse()
        {
            var payment = Fakers.Payment.Generate();

            var request = Fakers.GetPaymentDetailsRequest.Generate();
            request.PaymentId = payment.Id.ToString();

            _repositoryMock.Setup(m => m.GetById(payment.Id)).ReturnsAsync(payment);

            var response = await _handler.Handle(request, CancellationToken.None);

            response.Should().NotBeNull();

            response.Amount.Should().Be(payment.Amount);
            response.Currency.Should().Be(payment.Currency);
            response.Id.Should().Be(payment.Id.ToString());
            response.Status.Should().Be(payment.Status.ToString("G"));
            response.CreditCardNumber.Should().Be(payment.CreditCard.MaskedCardNumber);
            response.CreditCardExpiryDate.Should().Be(payment.CreditCard.ExpiryDate);
        }
    }
}
