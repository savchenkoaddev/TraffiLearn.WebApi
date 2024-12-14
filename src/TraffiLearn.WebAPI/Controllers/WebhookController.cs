using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Webhooks.Stripe.Commands;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/webhooks")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ISender _sender;

        public WebhookController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> HandleStripeWebhook(
            [FromHeader(Name = "Stripe-Signature")] string signatureHeader)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var command = new HandleStripeWebhookCommand(json, signatureHeader);

            var result = await _sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
