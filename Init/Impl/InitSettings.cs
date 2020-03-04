﻿using BaseTypes;
using Init.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Init.Impl
{
    public class InitSettings : BaseController, IInitSettings
    {
        private string _token;

        public string Token
        {
            get => _token;
            private set => SetValue(ref _token, value);
        }

        public InitSettings()
        {
            Token = "1013415129:AAHhl4vTbVwjh89BM-xAkVZV6UOxIRvPMNU";
        }

        public string GetToken([CallerMemberName] string name = "")
        {
            _logger.Information($"Получение токена в методе {name}");
            return Token;
        }
    }
}
