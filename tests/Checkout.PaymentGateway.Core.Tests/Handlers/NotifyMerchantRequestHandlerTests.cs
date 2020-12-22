using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Handlers;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Core.Tests.Common;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Handlers
{
    public class NotifyMerchantRequestHandlerTests
    {
        private Mock<IMerchantRepository> _repositoryMock;
        private Mock<IMerchantNotifier> _notifierMock;

        private NotifyMerchantRequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IMerchantRepository>();
            _notifierMock = new Mock<IMerchantNotifier>();
            _handler = new NotifyMerchantRequestHandler(_repositoryMock.Object, _notifierMock.Object);
        }

        [Test]
        public void Given_HandleMethod_When_NullRequestProvided_Should_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

        [Test]
        public void Given_HandleMethod_When_NoMerchantWithSpecifiedId_Should_ThrowException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(Fakers.IncomingNotifyMerchantRequest.Generate(), CancellationToken.None));
        }

        [Test]
        public async Task Given_HandleMethod_When_ValidRequestProvided_Should_HandleRequest()
        {
            var merchant = Fakers.Merchant.Generate();
            var incomingRequest = Fakers.IncomingNotifyMerchantRequest.Generate();

            incomingRequest.MerchantId = merchant.Id.ToString();

            _repositoryMock.Setup(m => m.GetById(merchant.Id))
                .ReturnsAsync(merchant);

            await _handler.Handle(incomingRequest, CancellationToken.None);

            _notifierMock.Verify(m => m.Notify(It.Is<Outgoing.NotifyMerchantRequest>(request =>
                request.CallbackUrl == merchant.CallbackApiUrl &&
                request.CallbackApiPassword == merchant.CallbackApiPassword &&
                request.CallbackApiUsername == merchant.CallbackApiUsername &&
                request.PaymentStatus == incomingRequest.PaymentStatus &&
                request.PaymentId == incomingRequest.PaymentId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
