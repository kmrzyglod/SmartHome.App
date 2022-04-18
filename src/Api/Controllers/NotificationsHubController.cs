using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Shared.Queries.General.NotificationsHubNegotiate;

namespace SmartHome.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
public class NotificationsHubController : BaseController
{
    public NotificationsHubController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Route("negotiate")]
    [ProducesResponseType(typeof(NegotiateResultVm), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Negotiate()
    {
        return Ok(await _mediator.Send(new NotificationsHubNegotiateQuery()));
    }
}