using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Tests.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Handlers
{
    [TestFixture]
    public class PaymentStatusUpdatedEventHandlerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<PaymentStatusUpdatedEventHandler>> _loggerMock;

        private PaymentStatusUpdatedEventHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PaymentStatusUpdatedEventHandler>>();
            _handler = new PaymentStatusUpdatedEventHandler(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Given_HandleMethod_When_NullNotificationProvided_Should_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public async Task Given_HandleMethod_When_ValidNotificationProvided_Should_SendRequest()
        {
            var @event = Fakers.PaymentStatusUpdatedEvent.Generate();

            await _handler.Handle(@event, CancellationToken.None);

            _mediatorMock.Verify(m => m.Send(It.Is<NotifyMerchantRequest>(
                        request => request.PaymentId == @event.PaymentId &&
                                   request.MerchantId == @event.MerchantId &&
                                   request.PaymentStatus == @event.PaymentStatus &&
                                   request.ProcessorStatus == @event.ProcessorStatus &&
                                   request.ProcessorTransactionId == @event.ProcessorTransactionId),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }
    }
}
