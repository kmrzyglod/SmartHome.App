using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.SendEmail;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.OutputActionExecutors
{
    public class SendEmailExecutor : IOutputActionExecutor
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;

        public SendEmailExecutor(IDateTimeProvider dateTimeProvider,
            IApplicationDbContext dbContext, IMediator mediator)
        {
            _dateTimeProvider = dateTimeProvider;
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public RuleOutputActionId OutputActionId => RuleOutputActionId.SendEmail;

        public async Task<bool> Execute(long ruleId, RuleOutputAction outputAction, CancellationToken cancellationToken)
        {
            var currentTime = _dateTimeProvider.GetUtcNow();

            var lastRuleExecution = await _dbContext.RulesExecutionHistory
                .Where(x => x.Rule.Id == ruleId)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastRuleExecution != default && (currentTime - lastRuleExecution).TotalHours < 3)
            {
                return false;
            }

            foreach (var sendEmailCommand in outputAction.Commands.Select(x => JsonSerializerHelpers.DeserializeFromObject<SendEmailCommand>(x))
                .Where(x => x != null))
            {
                await _mediator.Send(sendEmailCommand!, cancellationToken);
            }

            return true;
        }
    }
}