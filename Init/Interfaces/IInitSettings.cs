using Models;

namespace Init.Interfaces
{
    public interface IInitSettings
    {
        string GetToken();
        ProxyModel GetProxyData();
    }
}
