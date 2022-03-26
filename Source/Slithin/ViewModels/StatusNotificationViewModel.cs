using Slithin.Core;

namespace Slithin.ViewModels;

public class StatusNotificationViewModel : BaseViewModel
{
    private int _maxValue;
    private string _message;
    private int _value;

    public int MaxValue
    {
        get { return _maxValue; }
        set
        {
            SetValue(ref _maxValue, value);
        }
    }

    public string Message
    {
        get { return _message; }
        set { SetValue(ref _message, value); }
    }

    public int Value
    {
        get { return _value; }
        set { SetValue(ref _value, value); }
    }
}
