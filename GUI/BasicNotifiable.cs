using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI;

public abstract class BasicNotifiable : INotifyPropertyChanged
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
}