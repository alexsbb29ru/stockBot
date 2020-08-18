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
        AdminCommands,
        [Description("/dayStat")]
        DayStat,
        [Description("/writePost")]
        WritePost,
        [Description("/WpAdm")]
        WpAdm
    }
}