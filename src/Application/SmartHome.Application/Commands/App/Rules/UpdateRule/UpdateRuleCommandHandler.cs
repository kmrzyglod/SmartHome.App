using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Application.Commands.App.Rules.UpdateRule
{
    public class UpdateRuleCommandHandler : IRequestHandler<UpdateRuleCommand, CommandResultEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;

        public UpdateRuleCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResultEvent> Handle(UpdateRuleCommand request, CancellationToken cancellationToken)
        {
            var rule = await _dbContext.Rules.SingleAsync(x => x.Id == request.Id, cancellationToken);
            rule.Name = request.Name;
            rule.Body = JsonSerializer.Serialize(request.Body);
            rule.OutputAction = JsonSerializer.Serialize(request.OutputAction);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CommandResultEvent.CreateAppCommandResult(request, StatusCode.Success,
                _dateTimeProvider.GetUtcNow());
        }
    }
}