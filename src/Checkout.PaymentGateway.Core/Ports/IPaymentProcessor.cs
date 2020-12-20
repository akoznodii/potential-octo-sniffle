using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Models;
using Checkout.PaymentGateway.Core.Outgoing;

namespace Checkout.PaymentGateway.Core.Ports
{
    public interface IPaymentProcessor
    {
        Task<ProcessPaymentResponse> Process(ProcessPaymentRequest request, CancellationToken cancellationToken);

        PaymentStatus MapStatus(string status);
    }
}
