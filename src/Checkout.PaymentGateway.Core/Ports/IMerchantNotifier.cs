using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Outgoing;

namespace Checkout.PaymentGateway.Core.Ports
{
    public interface IMerchantNotifier
    {
        Task Notify(NotifyMerchantRequest request, CancellationToken cancellationToken);
    }
}
