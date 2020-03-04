using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Api.Controllers
{
    [ApiVersion("1.0")]
    public class EventLogController : BaseController
    {
        public EventLogController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEvent()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            return Ok();
        }
    }
}