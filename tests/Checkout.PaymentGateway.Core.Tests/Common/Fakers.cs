using System;
using Bogus;
using Checkout.PaymentGateway.Core.Events;
using Checkout.PaymentGateway.Core.Incoming;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Tests.Common
{
    public static class Fakers
    {
        public static Faker<CreditCard> CreditCard { get; } = new Faker<CreditCard>()
            .RuleFor(f => f.CardNumber, f => f.Finance.CreditCardNumber())
            .RuleFor(f => f.ExpiryMonth, f => f.Date.Future().Month.ToString("00"))
            .RuleFor(f => f.ExpiryYear, f => (f.Date.Future().Year % 100).ToString("00"));

        public static Faker<Payment> Payment { get; } = new Faker<Payment>()
            .RuleFor(f => f.Id, f => Guid.NewGuid())
            .RuleFor(f => f.Amount, f => f.Finance.Amount(1, 1000))
            .RuleFor(f => f.Currency, f => f.Finance.Currency().Code)
            .RuleFor(f => f.Status, f => f.PickRandom<PaymentStatus>())
            .RuleFor(f => f.MerchantId, f => Guid.NewGuid())
            .RuleFor(f => f.CreditCard, f => CreditCard.Generate());

        public static Faker<UpdatePaymentStatusRequest> UpdatePaymentStatusRequest { get; } =  new Faker<UpdatePaymentStatusRequest>()
            .RuleFor(f => f.RawStatus, f => f.Random.Word())
            .RuleFor(f=>f.TransactionId, f=> f.Random.Int().ToString());


        public static Faker<GetPaymentDetailsRequest> GetPaymentDetailsRequest { get; } =  new Faker<GetPaymentDetailsRequest>()
            .RuleFor(f => f.PaymentId, f => Guid.NewGuid().ToString());

        public static Faker<ProcessPaymentRequest> IncomingProcessPaymentRequest { get; } =
            new Faker<ProcessPaymentRequest>()
                .RuleFor(f => f.Amount, f => f.Finance.Amount(1, 1000))
                .RuleFor(f => f.Currency, f => f.Finance.Currency().Code)
                .RuleFor(f => f.CardNumber, f => f.Finance.CreditCardNumber())
                .RuleFor(f => f.CvvNumber, f => f.Finance.CreditCardCvv())
                .RuleFor(f => f.MerchantId, f => Guid.NewGuid())
                .RuleFor(f => f.ExpiryMonth, f => f.Date.Future().Month.ToString("00"))
                .RuleFor(f => f.ExpiryYear, f => (f.Date.Future().Year % 100).ToString("00"));

        public static Faker<Outgoing.ProcessPaymentResponse> OutgoingProcessPaymentResponse { get; } =
            new Faker<Outgoing.ProcessPaymentResponse>()
                .RuleFor(f => f.MappedStatus,
                    f => f.PickRandom(PaymentStatus.Pending, PaymentStatus.Failed))
                .RuleFor(f => f.TransactionId, f => Guid.NewGuid().ToString())
                .RuleFor(f => f.RawStatus, f => f.Random.Word());

        public static Faker<PaymentStatusUpdatedEvent> PaymentStatusUpdatedEvent { get; } =
            new Faker<PaymentStatusUpdatedEvent>()
                .RuleFor(f => f.PaymentId, f => Guid.NewGuid().ToString())
                .RuleFor(f => f.MerchantId, f => Guid.NewGuid().ToString())
                .RuleFor(f => f.PaymentStatus, f => f.PickRandom<PaymentStatus>().ToString("G"))
                .RuleFor(f => f.ProcessorStatus, f => f.Random.Word())
                .RuleFor(f => f.ProcessorTransactionId, f => f.Random.Int(1).ToString());

        public static Faker<NotifyMerchantRequest> IncomingNotifyMerchantRequest { get; } =
            new Faker<NotifyMerchantRequest>()
                .RuleFor(f => f.MerchantId, f => Guid.NewGuid().ToString())
                .RuleFor(f => f.PaymentId, f => Guid.NewGuid().ToString())
                .RuleFor(f => f.PaymentStatus, f => f.PickRandom<PaymentStatus>().ToString())
                .RuleFor(f => f.ProcessorStatus, f => f.Random.Word())
                .RuleFor(f => f.ProcessorTransactionId, f => f.Random.Int(1).ToString());

        public static Faker<Merchant> Merchant { get; } =
            new Faker<Merchant>()
                .RuleFor(f => f.PaymentStatusCallbackUrl, f => f.Internet.Url())
                .RuleFor(f => f.Id, f => Guid.NewGuid());
    }
}
