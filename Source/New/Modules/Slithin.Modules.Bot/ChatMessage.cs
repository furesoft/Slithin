using Slithin.Core.MVVM;

namespace Slithin.Modules.Bot;

public class ChatMessage : BaseViewModel
{
    private bool _isWriting;
    public bool SentByMe { get; set; }
    public string Username { get; set; }
    public object Content { get; set; }

    public string Hint { get; set; }

    public bool IsWriting
    {
        get => _isWriting;
        set => this.SetValue(ref _isWriting, value);
    }
}
