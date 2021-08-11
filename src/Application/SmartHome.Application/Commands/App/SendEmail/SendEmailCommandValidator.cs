using FluentValidation;
using SmartHome.Application.BaseValidators;
using SmartHome.Application.Shared.Commands.SendEmail;

namespace SmartHome.Application.Commands.App.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            Include(new CommandValidatorBase());
            RuleFor(x => x.RecipientEmail).EmailAddress();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}