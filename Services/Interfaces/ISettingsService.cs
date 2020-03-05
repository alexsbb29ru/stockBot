using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Services.Interfaces
{
    public interface ISettingsService
    {
        string GetTelegramToken([CallerMemberName] string name = "");
    }
}
