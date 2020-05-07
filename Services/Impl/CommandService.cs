using System;
using System.Collections.Generic;
using BaseTypes;
using Models.Enities;
using Services.Interfaces;

namespace Services.Impl
{
    public class CommandService : ICommandService
    {
        private readonly IUserService<Users, Guid> _userService;
        private readonly ILocalizeService _localizeService;
        
        private readonly Dictionary<string, Func<string>> _commandsDictionary;
        private string _lang;
        public CommandService(IUserService<Users, Guid> userService, ILocalizeService localizeService)
        {
            _userService = userService;
            _localizeService = localizeService;

            _commandsDictionary = new Dictionary<string, Func<string>>
            {
                {
                    BotCommands.Start.GetDescription(),
                    () => _localizeService[MessagesLangEnum.StartText.GetDescription(), _lang]
                },
                {
                    BotCommands.AdminCommands.GetDescription(),
                    () => { return ""; }
                }
            };

        }
        public string GetCommand(string messageCommand, string lang)
        {
            _lang = lang;
            return _commandsDictionary[messageCommand].Invoke();
        }
    }
}