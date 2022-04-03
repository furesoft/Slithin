using System.ComponentModel;
using System.Runtime.CompilerServices;
using Slithin.Core;

namespace Slithin.Core.MVVM;

public class NotifyObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnChange([CallerMemberName] string property = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    protected void SetValue<T>(ref T field, T value, [CallerMemberName] string property = null)
    {
        if (Equals(field, value))
            return;

        field = value;

        OnChange(property);
    }
}
