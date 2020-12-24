using Checkout.MerchantMock.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkout.MerchantMock.Api.Controllers
{
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("api/notifications")]
        public IActionResult ReceiveNotification(NotificationRequest notification)
        {
            _logger.LogInformation("Received notification from payment gateway. PaymentId: {PaymentId}; Status: {PaymentStatus}; TransactionId: {TransactionId}",
                notification.PaymentId,
                notification.PaymentStatus,
                notification.ProcessorTransactionId);

            return Ok();
        }
    }
}
