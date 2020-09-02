namespace SmartHome.Application.Shared.Interfaces.Command
{
    public interface IDeviceCommand: ICommand
    {
        public string TargetDeviceId { get; set; }
    }
}
