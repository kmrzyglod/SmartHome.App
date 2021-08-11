using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.EmailSender;
using SmartHome.Application.Shared.Commands.SendEmail;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Application.Commands.App.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, CommandResultEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailSender _emailSender;

        public SendEmailCommandHandler(IEmailSender emailSender, IDateTimeProvider dateTimeProvider)
        {
            _emailSender = emailSender;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResultEvent> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            await _emailSender.SendPlainText(request.RecipientEmail, request.Subject, request.Content);
            return CommandResultEvent.CreateAppCommandResult(request, StatusCode.Success,
                _dateTimeProvider.GetUtcNow());
        }
    }
}