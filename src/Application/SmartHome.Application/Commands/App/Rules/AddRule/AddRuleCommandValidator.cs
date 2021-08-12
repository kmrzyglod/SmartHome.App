using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.Rules.AddRule;

namespace SmartHome.Application.Commands.App.Rules.AddRule
{
    public class AddRuleCommandValidator : AbstractValidator<AddRuleCommand>
    {
        public AddRuleCommandValidator()
        {
            Include(new CommandValidatorBase());
            //TODO add body & output action validation
        }
    }
}