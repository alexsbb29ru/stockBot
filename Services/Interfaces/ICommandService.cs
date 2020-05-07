namespace Services.Interfaces
{
    public interface ICommandService
    {
        string GetCommand(string messageCommand, string lang);
    }
}