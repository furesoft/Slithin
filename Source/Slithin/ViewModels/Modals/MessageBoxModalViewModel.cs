namespace Slithin.ViewModels.Modals;

public class MessageBoxModalViewModel : ModalBaseViewModel
{
    private string _message;

    public string Message
    {
        get => _message;
        set => SetValue(ref _message, value);
    }
}
