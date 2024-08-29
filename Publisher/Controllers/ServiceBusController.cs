using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Publisher.Models;
using Publisher.Services;

namespace Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBusController : ControllerBase
    {
        public IServiceBusService? ServiceBusService { get; set; }

        public ServiceBusController(IServiceBusService ServiceBusService)
        {
            this.ServiceBusService = ServiceBusService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] ServiceBusRequestModel request)
        {
            if (request == null || string.IsNullOrEmpty(request.TopicName) || request.Message == null)
            {
                return BadRequest("Invalid request data.");
            }

            await ServiceBusService.SendMessageAsync(request.Message, request.TopicName);
            return Ok("Message sent successfully.");
        }
    }
}
