using SmartHome.Application.Shared.Interfaces.Message;

namespace SmartHome.Application.Shared.Interfaces.Command
{
    public interface ICommand : IMessage
    {
        public string CommandName { get;}
    }
}