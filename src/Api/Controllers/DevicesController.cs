using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Interfaces.CommandBus;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatusHistory;

namespace SmartHome.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    public class DevicesController : BaseController
    {
        private readonly ICommandBus _commandBus;

        public DevicesController(IMediator mediator, ICommandBus commandBus) : base(mediator)
        {
            _commandBus = commandBus;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(PaginationResult<DeviceListEntryVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDeviceList([FromQuery] GetDeviceListQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        
        [HttpGet]
        [Route("status")]
        [ProducesResponseType(typeof(DeviceStatusVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDeviceStatus([FromQuery] GetDeviceStatusQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("status/history")]
        [ProducesResponseType(typeof(PaginationResult<DeviceStatusHistoryVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDeviceStatusHistory([FromQuery] GetDeviceStatusHistoryQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        #region shared commands
       
        [HttpPost]
        [Route("commands/ping")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendPingCommand([FromBody] PingCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpPost]
        [Route("commands/diagnostic-data")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendDiagnosticDataCommand([FromBody] SendDiagnosticDataCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        #endregion

        #region window manager commands
       
        [HttpPost]
        [Route("window-manager/commands/open-window")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendWindowManagerOpenWindowCommand([FromBody] OpenWindowCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpPost]
        [Route("window-manager/commands/close-window")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendWindowManagerCloseWindowCommand([FromBody] CloseWindowCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        #endregion

        #region greenhouse controller commands
       
        [HttpPost]
        [Route("greenhouse-controller/commands/irrigate")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendGreenhouseControllerIrrigateCommand([FromBody] IrrigateCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpPost]
        [Route("greenhouse-controller/commands/abort-irrigation")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendGreenhouseControllerAbortIrrigationCommand([FromBody] AbortIrrigationCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        #endregion


    }
}