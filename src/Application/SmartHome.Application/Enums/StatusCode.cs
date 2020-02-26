namespace SmartHome.Application.Enums
{
    public enum StatusCode
    {
        //When command processed sucessfully
        Success,

        //When error occured during command execution
        Error,

        //Command/query execution refused e.g. same command is currently executing on device, 
        Refused
    }
}