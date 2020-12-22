using System.Collections.Generic;
using Checkout.PaymentGateway.Core.Models;

namespace Checkout.PaymentGateway.Core.Options
{
    public class PaymentOptions
    {
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public IReadOnlyList<string> CurrencyCodes { get; set; }
        public IReadOnlyList<string> CardTypes { get; set; }

        public static PaymentOptions CreateDefault()
        {
            return new PaymentOptions
            {
                MinAmount = 1m,
                MaxAmount = 10000m,
                CurrencyCodes = new List<string> {"EUR", "USD", "CZK", "PLN"},
                CardTypes = new List<string> {CreditCardTypes.Visa, CreditCardTypes.Mastercard},
            };
        }
    }
}
