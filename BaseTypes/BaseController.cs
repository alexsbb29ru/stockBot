using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BaseTypes
{
    public class BaseController : INotifyPropertyChanged
    {
        //Logger initialization
        protected ILogger _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        protected void SetValue<T>(ref T property, T value, [CallerMemberName] string propName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                _logger.Information($"Изменено свойство {propName}. Старое значение: {property}. Новое значение:{value}");
                property = value;
                OnPropertyChanged(propName);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

