using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;

namespace SmartHome.Application.Commands.App.Rules.UpdateRule
{
    public class UpdateRuleCommandValidator : AbstractValidator<UpdateRuleCommand>
    {
        public UpdateRuleCommandValidator(IApplicationDbContext dbContext)
        {
            Include(new CommandValidatorBase());
            RuleFor(x => x.Id).CustomAsync(async (id, context, cancellationToken) =>
            {
                bool isExist = await dbContext.Rules.AnyAsync(x => x.Id == id, cancellationToken);
                if (!isExist)
                {
                    context.AddFailure(nameof(UpdateRuleCommand.Id), $"Rule with Id {id} not exists");
                }
            });
            //TODO add body & output action validation
        }
    }
}