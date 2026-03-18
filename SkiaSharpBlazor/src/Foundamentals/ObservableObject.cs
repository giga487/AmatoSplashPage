using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Foundamentals
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                PropertyChangedEventArgs eventArgs = new PropertyChangedEventArgs(propertyName);
                eventHandler.Invoke(this, eventArgs);
            }
        }

        protected virtual bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            bool areEqual = EqualityComparer<T>.Default.Equals(backingStore, value);
            if (areEqual)
            {
                return false;
            }

            backingStore = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
