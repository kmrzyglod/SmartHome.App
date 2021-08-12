using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Events.App
{
    public class RuleExecutionResultEventHandler:  INotificationHandler<RuleExecutionResultEvent>
    {
        private readonly IApplicationDbContext _dbContext;

        public RuleExecutionResultEventHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Handle(RuleExecutionResultEvent notification, CancellationToken cancellationToken)
        {
            _dbContext.RulesExecutionHistory.Add(new RuleExecutionHistory
            {
                RuleId = notification.RuleId,
                ErrorMessage = notification.ErrorMessage,
                ExecutionStatus = notification.ExecutionStatus,
                Timestamp = notification.Timestamp
            });

            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
