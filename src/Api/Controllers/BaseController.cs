using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        protected BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}