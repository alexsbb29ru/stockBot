using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ILocalizeService : IStringLocalizer
    {
        LocalizedString this[string name, string lang] { get; }
    }
}
