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
        /// <summary>
        /// Set new value to property
        /// </summary>
        /// <typeparam name="T">type of property</typeparam>
        /// <param name="property">target property</param>
        /// <param name="value">new value</param>
        /// <param name="propName">name of target property</param>
        protected void SetValue<T>(ref T property, T value, [CallerMemberName] string propName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                _logger.Information($"Изменено свойство {propName}. Старое значение: {property}. Новое значение:{value}");
                property = value;
                OnPropertyChanged(propName);
            }
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// On property changed method
        /// </summary>
        /// <param name="propName"></param>
        protected void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

