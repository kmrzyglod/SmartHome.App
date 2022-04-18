using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Interfaces.CommandBus;
using SmartHome.Application.Shared.Commands.Rules.AddRule;
using SmartHome.Application.Shared.Commands.Rules.DeleteRule;
using SmartHome.Application.Shared.Commands.Rules.SetRuleState;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRuleDetails;
using SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory;
using SmartHome.Application.Shared.Queries.Rules.GetRulesExecutionHistoryList;
using SmartHome.Application.Shared.Queries.Rules.GetRulesList;

namespace SmartHome.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    public class RulesController : BaseController
    {
        private readonly ICommandBus _commandBus;

        public RulesController(IMediator mediator, ICommandBus commandBus) : base(mediator)
        {
            _commandBus = commandBus;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(PaginationResult<RulesListEntryVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRulesList([FromQuery] GetRulesListQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("execution-history")]
        [ProducesResponseType(typeof(PaginationResult<RuleExecutionHistoryVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRuleExecutionHistory([FromQuery] GetRuleExecutionHistoryQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("execution-history/list")]
        [ProducesResponseType(typeof(PaginationResult<RulesExecutionHistoryListVm>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRulesExecutionHistoryList([FromQuery] GetRulesExecutionHistoryListQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        [Route("details")]
        [ProducesResponseType(typeof(RuleDetailsVm), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRuleDetails([FromQuery] GetRuleDetailsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CommandCorrelationId), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRule([FromBody] AddRuleCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CommandCorrelationId), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRule([FromBody] UpdateRuleCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(CommandCorrelationId), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRule([FromQuery] DeleteRuleCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }

        [HttpPatch]
        [Route("state")]
        [ProducesResponseType(typeof(CommandCorrelationId), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRule([FromBody] SetRuleStateCommand command)
        {
            return Ok(await _commandBus.SendAsync(command));
        }
    }
}