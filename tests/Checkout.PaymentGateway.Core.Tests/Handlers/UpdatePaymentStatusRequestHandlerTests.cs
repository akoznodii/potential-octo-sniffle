using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Checkout.PaymentGateway.Core.Events;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Core.Tests.Common;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Handlers
{
    [TestFixture]
    public class UpdatePaymentStatusRequestHandlerTests
    {
        private Mock<IPaymentRepository> _repositoryMock;
        private Mock<IPaymentProcessor> _processorMock;
        private Mock<IMediator> _mediatorMock;

        private UpdatePaymentStatusRequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IPaymentRepository>();
            _processorMock = new Mock<IPaymentProcessor>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new UpdatePaymentStatusRequestHandler(_repositoryMock.Object, _processorMock.Object, _mediatorMock.Object);
        }

        [Test]
        public void Given_HandleMethod_When_NullRequestProvided_Should_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public void Given_HandleMethod_When_NoPaymentWithTransactionId_Should_ThrowException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(Fakers.UpdatePaymentStatusRequest.Generate(), CancellationToken.None));
        }

        [TestCase(PaymentStatus.Initial, PaymentStatus.Approved, true)]
        [TestCase(PaymentStatus.Pending, PaymentStatus.Approved, true)]
        [TestCase(PaymentStatus.Approved, PaymentStatus.Approved, false)]
        [TestCase(PaymentStatus.Declined, PaymentStatus.Approved, false)]
        [TestCase(PaymentStatus.Failed, PaymentStatus.Approved, true)]
        [TestCase(PaymentStatus.Failed, PaymentStatus.Declined, true)]
        [TestCase(PaymentStatus.Failed, PaymentStatus.Failed, false)]
        [TestCase(PaymentStatus.Declined, PaymentStatus.Declined, false)]
        public async Task Given_HandleMethod_When_PaymentWithStatus_Should_HandleRequest(PaymentStatus paymentStatus, PaymentStatus notificationStatus, bool shouldUpdateAndRaiseEvent)
        {
            // Arrange
            var payment = Fakers.Payment.Generate();
            payment.Status = paymentStatus;
            var request = Fakers.UpdatePaymentStatusRequest.Generate();

            _repositoryMock.Setup(m => m.GetByProcessorTransactionId(request.TransactionId))
                .ReturnsAsync(payment);

            _repositoryMock.Setup(m => m.Update(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            _processorMock.Setup(m =>
                    m.MapStatus(It.IsAny<string>()))
                .Returns(notificationStatus);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(m => m.GetByProcessorTransactionId(It.IsAny<string>()), Times.Once);

            var times = shouldUpdateAndRaiseEvent ? Times.Once() : Times.Never();

            _repositoryMock.Verify(m => m.Update(It.IsAny<Payment>()), times);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<PaymentStatusUpdatedEvent>(), It.IsAny<CancellationToken>()), times);
        }
    }
}
