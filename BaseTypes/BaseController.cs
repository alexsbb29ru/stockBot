using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BaseTypes
{
    public class BaseController : INotifyPropertyChanged
    {
        protected void SetValue<T>(ref T property, T value, [CallerMemberName] string propName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                OnPropertyChanged(nameof(property));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

