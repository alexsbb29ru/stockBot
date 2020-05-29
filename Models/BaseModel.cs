using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    public class BaseModel  : INotifyPropertyChanged
    {
        /// <summary>
        /// Set new value to property
        /// </summary>
        /// <typeparam name="T">type of property</typeparam>
        /// <param name="property">target property</param>
        /// <param name="value">new value</param>
        /// <param name="propName">name of target property</param>
        protected void SetValue<T>(ref T property, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, value)) return;
            OnPropertyChanged(propName);
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// On property changed method
        /// </summary>
        /// <param name="propName"></param>
        private void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}