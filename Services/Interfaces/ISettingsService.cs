using System.Runtime.CompilerServices;

namespace Services.Interfaces
{
    public interface ISettingsService
    {
        string GetTelegramToken([CallerMemberName] string name = "");
    }
}
