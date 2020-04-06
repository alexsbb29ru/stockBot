using Models;
using System.Runtime.CompilerServices;

namespace Services.Interfaces
{
    public interface ISettingsService
    {
        string GetTelegramToken([CallerMemberName] string name = "");
        ProxyModel GetProxyConfig([CallerMemberName] string name = "");
    }
}
