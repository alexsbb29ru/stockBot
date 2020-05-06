using System.ComponentModel;

namespace BaseTypes
{
    public enum BotCommands
    {
        [Description("/start")]
        Start,
        [Description("/getuserscount")]
        UsersCount,
        [Description("/admcom")]
        AdminCommands
    }
}