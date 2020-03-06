﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Commands.Devices.Shared.Ping;
using SmartHome.Application.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Interfaces.CommandBus;
using SmartHome.Application.Models;
using SmartHome.Application.Queries.Devices.Shared.GetDeviceList;
using SmartHome.Application.Queries.Devices.Shared.GetDeviceStatus;
using SmartHome.Application.Queries.Devices.Shared.GetDeviceStatusHistory;

namespace SmartHome.Api.Controllers
{
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

        //TODO consider do this in more generic way
        #region commands
       
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
    }
}