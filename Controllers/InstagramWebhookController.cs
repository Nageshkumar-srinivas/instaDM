using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace instaDM.Controllers
{
    [Route("api/webhooks/instagram")]
    [ApiController]
    public class InstagramWebhookController : ControllerBase
    {
        private readonly ILogger<InstagramWebhookController> _logger;

        public InstagramWebhookController(ILogger<InstagramWebhookController> logger)
        {
            _logger = logger;
        }

        // Meta platform webhook verification
        [HttpGet]
        public IActionResult VerifyWebhook([FromQuery(Name = "hub.mode")] string mode,
                                           [FromQuery(Name = "hub.verify_token")] string verifyToken,
                                           [FromQuery(Name = "hub.challenge")] string challenge)
        {
            const string expectedToken = "my_secret_verify_token"; // Set your secret token here

            if (mode == "subscribe" && verifyToken == expectedToken)
            {
                _logger.LogInformation("✅ Webhook verified.");
                return Ok(challenge);
            }

            _logger.LogWarning("❌ Webhook verification failed.");
            return Unauthorized();
        }

        // Webhook POST handler for comment events
        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            _logger.LogInformation("📩 Webhook received: {Body}", body);

            try
            {
                var json = JsonDocument.Parse(body);
                // You can parse the structure here later
            }
            catch (Exception ex)
            {
                _logger.LogError("Parsing error: {Message}", ex.Message);
            }

            return Ok();
        }
    }
}
