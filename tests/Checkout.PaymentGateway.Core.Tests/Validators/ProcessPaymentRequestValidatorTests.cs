using System;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Options;
using Checkout.PaymentGateway.Core.Ports;
using Checkout.PaymentGateway.Core.Tests.Common;
using Checkout.PaymentGateway.Core.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Core.Tests.Validators
{
    [TestFixture]
    public class ProcessPaymentRequestValidatorTests
    {
        private PaymentOptions _paymentOptions;
        private Mock<IOptionsSnapshot<PaymentOptions>> _optionsMock;
        private Mock<IMerchantRepository> _repositoryMock;
        private ProcessPaymentRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _paymentOptions = PaymentOptions.CreateDefault();

            _optionsMock = new Mock<IOptionsSnapshot<PaymentOptions>>();
            _optionsMock.Setup(m => m.Value).Returns(_paymentOptions);

            _repositoryMock = new Mock<IMerchantRepository>();

            _repositoryMock
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(Fakers.Merchant.Generate());

            _validator = new ProcessPaymentRequestValidator(_optionsMock.Object, _repositoryMock.Object);
        }

        [Test]
        public void When_ValidRequestProvided_Should_NotHaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            var result = _validator.TestValidate(request);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void When_MerchantNotFound_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            _repositoryMock
                .Setup(m => m.GetById(Guid.Parse(request.MerchantId)))
                .ReturnsAsync((Merchant)null);

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.MerchantId);
        }

        [Test]
        public void When_InvalidAmount_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.Amount = -1;

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Amount);
        }

        [Test]
        public void When_InvalidCurrency_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.Currency = "UAH";

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Currency);
        }

        [Test]
        public void When_InvalidCardType_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.CardType = "AMERICANEXPRESS";

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.CardType);
        }

        [Test]
        public void When_InvalidCvvNumber_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.CvvNumber = "INVALID";

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.CvvNumber);
        }

        [Test]
        public void When_InvalidExpiryMonth_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.ExpiryMonth = 13;

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.ExpiryMonth);
        }

        [Test]
        public void When_InvalidExpiryYear_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            request.ExpiryYear = -1;

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.ExpiryYear);
        }

        [Test]
        public void When_InvalidExpiryDate_Should_HaveValidationError()
        {
            var request = Fakers.IncomingProcessPaymentRequest.Generate();

            var date = DateTime.Today.AddMonths(-1);

            request.ExpiryMonth = date.Month;
            request.ExpiryYear = date.Year;

            var result = _validator.TestValidate(request);

            result.IsValid.Should().BeFalse();

            result.ToString().Should().Be("Credit card is expired.");
        }
    }
}
