using Microsoft.Extensions.Localization;

namespace Services.Interfaces
{
    public interface ILocalizeService : IStringLocalizer
    {
        LocalizedString this[string name, string lang] { get; }
    }
}
