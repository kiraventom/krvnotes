using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Utils
{
    public abstract class Notifiable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);
            if (PropertyChanged == null)
                return;

            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        // ReSharper disable once RedundantAssignment
        protected void SetAndRaise<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
