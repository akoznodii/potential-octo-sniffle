using System.Linq;

namespace Checkout.PaymentGateway.Core.Models
{
    public class CreditCard
    {
        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string MaskedCardNumber => $"************{string.Concat(CardNumber.TakeLast(4))}";

        public string ExpiryDate => $"{ExpiryMonth}/{string.Concat(ExpiryYear.TakeLast(2))}";
    }
}
