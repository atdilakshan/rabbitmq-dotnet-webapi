using Catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    public class EventTestController(IEventTestService _eventTestService) : Controller
    {
        [HttpGet("QueueExchange")]
        public async Task<IActionResult> QueueExchange()
        {
            try
            {
                await _eventTestService.PublishViaQueue();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TopicExchange")]
        public async Task<IActionResult> TopicExchange()
        {
            try
            {
                await _eventTestService.PublishViaTopic();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FanOutExchange")]
        public async Task<IActionResult> FanOutExchange()
        {
            try
            {
                await _eventTestService.PublishViaFanOut();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
