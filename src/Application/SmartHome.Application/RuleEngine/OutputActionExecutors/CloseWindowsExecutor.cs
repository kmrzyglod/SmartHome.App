using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;
using SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Application.Shared.RulesEngine.Models;
using SmartHome.Domain.Const;

namespace SmartHome.Application.RuleEngine.OutputActionExecutors
{
    public class CloseWindowsExecutor : IOutputActionExecutor
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;
        private readonly IEventStoreClient _eventStoreClient;
        private readonly IMediator _mediator;

        public CloseWindowsExecutor(IDateTimeProvider dateTimeProvider, IEventStoreClient eventStoreClient,
            IApplicationDbContext dbContext, IMediator mediator)
        {
            _dateTimeProvider = dateTimeProvider;
            _eventStoreClient = eventStoreClient;
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public RuleOutputActionId OutputActionId => RuleOutputActionId.CloseWindows;

        public async Task<bool> Execute(long ruleId, RuleOutputAction outputAction, CancellationToken cancellationToken)
        {
            var currentTime = _dateTimeProvider.GetUtcNow();

            var lastRuleExecution = await _dbContext.RulesExecutionHistory
                .Where(x => x.Rule.Id == ruleId)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastRuleExecution != default && (currentTime - lastRuleExecution).TotalMinutes <= 35)
            {
                return false;
            }

            var windowsStatusEvt = await _eventStoreClient.FindEventsByCriteriaAsync(new GetEventsQuery
            {
                Source = DeviceIds.WindowsController,
                EventName = nameof(WindowsControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1,
                From = _dateTimeProvider.GetUtcNow().AddDays(-1)
            }, cancellationToken);

            if (windowsStatusEvt.ResultTotalCount == 0)
            {
                return false;
            }

            var lastEvent = windowsStatusEvt.Result.First();
            var evt = lastEvent.EventData as WindowsControllerTelemetryEvent;
            bool isWindow1Opened = evt!.WindowsStatus[0];
            bool isWindow2Opened = evt.WindowsStatus[1];

            var commands = outputAction.Commands.Select(x => JsonSerializerHelpers.DeserializeFromObject<CloseWindowCommand>(x)).ToList();
            int executedCommands = 0;
            if (isWindow1Opened && commands.Any(x => x is {WindowId: 0}))
            {
                await _mediator.Send(commands.First(x => x is {WindowId: 0})!, cancellationToken);
                executedCommands++;
            }

            if (isWindow2Opened && commands.Any(x => x is {WindowId: 1}))
            {
                await _mediator.Send(commands.First(x => x is {WindowId: 1})!, cancellationToken);
                executedCommands++;
            }

            return executedCommands > 0;
        }
    }
}