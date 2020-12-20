using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Core.Tests.Common;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Handlers
{
    [TestFixture]
    public class ProcessPaymentRequestHandlerTests
    {
        private Mock<IPaymentRepository> _repositoryMock;
        private Mock<IPaymentProcessor> _processorMock;

        private ProcessPaymentRequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IPaymentRepository>();
            _processorMock = new Mock<IPaymentProcessor>();

            _handler = new ProcessPaymentRequestHandler(_repositoryMock.Object, _processorMock.Object);
        }

        [Test]
        public void Given_HandleMethod_When_NullRequestProvided_Should_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public async Task Given_HandleMethod_When_ValidRequestProvided_Should_HandleRequest()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            var outgoingResponse = Fakers.OutgoingProcessPaymentResponse.Generate();

            _repositoryMock.Setup(m => m.Add(It.IsAny<Payment>()))
                .Returns<Payment>(payment => {
                    payment.Id = paymentId;
                    return Task.CompletedTask;
                });

            _repositoryMock.Setup(m => m.Update(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            _processorMock.Setup(m =>
                    m.Process(It.IsAny<Outgoing.ProcessPaymentRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(outgoingResponse);

            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.PaymentId.Should().Be(paymentId);

            _repositoryMock.Verify(m => m.Add(It.IsAny<Payment>()), Times.Once);
            _repositoryMock.Verify(m => m.Update(It.IsAny<Payment>()), Times.Once);
            _processorMock.Verify(m => m.Process(It.IsAny<Outgoing.ProcessPaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
