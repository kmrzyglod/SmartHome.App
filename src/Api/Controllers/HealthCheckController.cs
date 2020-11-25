using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Api.Controllers
{
    [ApiVersion("1.0")]
    public class HealthCheckController : BaseController
    {
        public HealthCheckController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public IActionResult GetHealthStatus()
        {
            return Ok();
        }
    }
}