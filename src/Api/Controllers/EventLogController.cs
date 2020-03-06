using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Models;
using SmartHome.Application.Queries.App.GetEvents;
using SmartHome.Application.Queries.Devices.Shared.GetDeviceList;

namespace SmartHome.Api.Controllers
{
    [ApiVersion("1.0")]
    public class EventLogController : BaseController
    {
        public EventLogController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<EventVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEvents([FromQuery] GetEventsQuery query)
        {
            return Ok((await _mediator.Send(query)));
        }
    }
}