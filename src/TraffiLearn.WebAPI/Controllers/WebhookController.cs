using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ISender _sender;

        public WebhookController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var signatureHeader = Request.Headers["Stripe-Signature"];

            var result = await _sender.Send(new HandleStripeEventCommand(json, signatureHeader));

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
