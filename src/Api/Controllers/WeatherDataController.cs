using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity;
using SmartHome.Application.Shared.Queries.WeatherStation.GetInsolation;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPressure;
using SmartHome.Application.Shared.Queries.WeatherStation.GetTemperature;
using SmartHome.Application.Shared.Queries.WeatherStation.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindAggregates;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;

namespace SmartHome.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    public class WeatherDataController : BaseController
    {

        public WeatherDataController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("temperature")]
        [ProducesResponseType(typeof(List<TemperatureVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTemperature([FromQuery] GetTemperatureQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        
        [HttpGet]
        [Route("temperature/aggregates")]
        [ProducesResponseType(typeof(TemperatureAggregatesVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTemperatureAggregates([FromQuery] GetTemperatureAggregatesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("humidity")]
        [ProducesResponseType(typeof(List<HumidityVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHumidity([FromQuery] GetHumidityQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("pressure")]
        [ProducesResponseType(typeof(List<PressureVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPressure([FromQuery] GetPressureQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("wind")]
        [ProducesResponseType(typeof(List<WindParametersVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetWindParameters([FromQuery] GetWindParametersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("wind/aggregates")]
        [ProducesResponseType(typeof(WindAggregatesVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetWindAggregates([FromQuery] GetWindAggregatesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("precipitation")]
        [ProducesResponseType(typeof(List<PrecipitationVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPrecipitation([FromQuery] GetPrecipitationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("insolation")]
        [ProducesResponseType(typeof(List<InsolationVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLightLevel([FromQuery] GetInsolationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}