using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.Rules.AddRule;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Commands.App.Rules.AddRule
{
    public class AddRuleCommandHandler : IRequestHandler<AddRuleCommand, CommandResultEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;

        public AddRuleCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResultEvent> Handle(AddRuleCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Rules.AddAsync(new Rule
            {
                Name = request.Name,
                Body = JsonSerializer.Serialize(request.Body),
                OutputAction = JsonSerializer.Serialize(request.OutputAction),
                IsActive = true
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CommandResultEvent.CreateAppCommandResult(request, StatusCode.Success,
                _dateTimeProvider.GetUtcNow());
        }
    }
}