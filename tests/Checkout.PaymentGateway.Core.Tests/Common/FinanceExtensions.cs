using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.Extras;

namespace Checkout.PaymentGateway.Core.Tests.Common
{
    public static class FinanceExtensions
    {
        public static string CreditCardNumber(this Finance finance, int length)
        {
            var numberList = Enumerable.Repeat("#", length - 1).Select(_ => finance.Random.Number(9)).ToList();

            var checkNum = numberList.CheckDigit();
            return $"{string.Concat(numberList.Select(c => c.ToString()))}{checkNum.ToString()}";
        }
    }
}
