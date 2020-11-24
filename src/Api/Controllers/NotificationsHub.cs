using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Shared.Queries.General.NotificationsHubNegotiate;

namespace SmartHome.Api.Controllers
{
    [ApiVersion("1.0")]
    public class NotificationsHub: BaseController
    {
        public NotificationsHub(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpPost]
        [Route("negotiate")]
        [ProducesResponseType(typeof(NegotiateResultVm), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Negotiate()
        {
            return Ok(await _mediator.Send(new NotificationsHubNegotiateQuery()));
        }
    }
}
