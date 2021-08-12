using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.Rules.SetRuleState;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Application.Commands.App.Rules.SetRuleState
{
    public class SetRuleStateCommandHandler : IRequestHandler<SetRuleStateCommand, CommandResultEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;

        public SetRuleStateCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResultEvent> Handle(SetRuleStateCommand request, CancellationToken cancellationToken)
        {
            var rule = await _dbContext.Rules.SingleAsync(x => x.Id == request.Id, cancellationToken);
            rule.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CommandResultEvent.CreateAppCommandResult(request, StatusCode.Success,
                _dateTimeProvider.GetUtcNow());
        }
    }
}