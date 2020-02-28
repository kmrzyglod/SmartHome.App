namespace SmartHome.Application.Interfaces.Command
{
    public interface IDeviceCommand: ICommand
    {
        public string TargetDeviceId { get; set; }
    }
}
