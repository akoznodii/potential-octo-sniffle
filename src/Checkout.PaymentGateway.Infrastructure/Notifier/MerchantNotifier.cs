using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Core.Outgoing;
using Checkout.PaymentGateway.Core.Ports;
using RestSharp;
using RestSharp.Authenticators;

namespace Checkout.PaymentGateway.Infrastructure.Notifier
{
    public class MerchantNotifier : IMerchantNotifier
    {
        public async Task Notify(NotifyMerchantRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var client = new RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(request.CallbackApiUsername, request.CallbackApiPassword, Encoding.UTF8)
            };

            var outgoingRequest = new RestRequest(request.CallbackUrl);

            outgoingRequest.AddJsonBody(new
            {
                PaymentId = request.PaymentId,
                PaymentStatus = request.PaymentStatus,
                ProcessorTransactionId = request.ProcessorTransactionId
            });

            await client.ExecutePostAsync(outgoingRequest, cancellationToken);
        }
    }
}
