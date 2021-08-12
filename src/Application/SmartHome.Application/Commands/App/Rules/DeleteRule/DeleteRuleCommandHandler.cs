using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.Rules.DeleteRule;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Commands.App.Rules.DeleteRule
{
    public class DeleteRuleCommandHandler : IRequestHandler<DeleteRuleCommand, CommandResultEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;

        public DeleteRuleCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResultEvent> Handle(DeleteRuleCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Rules.Remove(new Rule
            {
                Id = request.Id
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CommandResultEvent.CreateAppCommandResult(request, StatusCode.Success,
                _dateTimeProvider.GetUtcNow());
        }
    }
}