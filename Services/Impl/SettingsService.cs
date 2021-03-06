﻿using BaseTypes;
using Init.Interfaces;
using Models;
using Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Services.Impl
{
    public class SettingsService : BaseController, ISettingsService
    {
        private IInitSettings _initSetting;
        public SettingsService(IInitSettings initSetting)
        {
            _initSetting = initSetting;
        }
        public string GetTelegramToken([CallerMemberName] string name = "")
        {
            Logger.Information($"Получение токена в методе {name}");
            return _initSetting.GetToken();
        }

        public ProxyModel GetProxyConfig([CallerMemberName] string name = "")
        {
            Logger.Information($"Получение конфигов прокси в методе {name}");
            return _initSetting.GetProxyData();
        }
    }
}
