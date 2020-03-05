﻿using BaseTypes;
using Init.Interfaces;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
            _logger.Information($"Получение токена в методе {name}");
            return _initSetting.GetToken();
        }

        public ProxyModel GetProxyConfig([CallerMemberName] string name = "")
        {
            _logger.Information($"Получение конфигов прокси в методе {name}");
            return _initSetting.GetProxyData();
        }
    }
}
