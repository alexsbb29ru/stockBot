using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Init.Interfaces
{
    public interface IInitSettings
    {
        string GetToken([CallerMemberName] string name = "");
    }
}
