using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Api.Controllers
{
    [ApiVersion("1.0")]
    public class GreenhouseDataController : BaseController
    {
        public GreenhouseDataController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("temperature")]
        [ProducesResponseType(typeof(List<TemperatureVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTemperature([FromQuery] GetTemperatureQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("temperature/aggregates")]
        [ProducesResponseType(typeof(TemperatureAggregatesVm), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTemperatureAggregates([FromQuery] GetTemperatureAggregatesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("humidity")]
        [ProducesResponseType(typeof(List<HumidityVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHumidity([FromQuery] GetHumidityQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("insolation")]
        [ProducesResponseType(typeof(List<InsolationVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetInsolation([FromQuery] GetInsolationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("soil-moisture")]
        [ProducesResponseType(typeof(List<SoilMoistureVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSoilMoisture([FromQuery] GetSoilMoistureQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("irrigation-data")]
        [ProducesResponseType(typeof(List<IrrigationDataVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetIrrigationData([FromQuery] GetIrrigationDataQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("windows-status")]
        [ProducesResponseType(typeof(WindowsStatusVm), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetWindowsStatus([FromQuery] GetWindowsStatusQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}