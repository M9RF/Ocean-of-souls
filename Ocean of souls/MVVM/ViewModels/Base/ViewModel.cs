using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ocean_of_souls.MVVM.ViewModels.Base
{
    class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// An event notifies the system that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the value of a property.
        /// </summary>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return true;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
