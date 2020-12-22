using System.Linq;

namespace Checkout.PaymentGateway.Core.Models
{
    public class CreditCard
    {
        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string MaskedCardNumber =>
            string.IsNullOrEmpty(CardNumber) ?
                null :
                $"****-****-****-{string.Concat(CardNumber.TakeLast(4))}";

        public string ExpiryDate => $"{ExpiryMonth:00}/{(ExpiryYear % 100):00}";
    }
}
