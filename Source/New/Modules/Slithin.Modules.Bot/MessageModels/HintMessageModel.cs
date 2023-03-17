using System.Windows.Input;

namespace Slithin.Modules.Bot.MessageModels;

public class HintMessageModel : SimpleMessageModel
{
    private bool _hintsVisible;
    public string[] Hints { get; set; }

    public ICommand Command { get; set; }

    public bool HintsVisible
    {
        get => _hintsVisible;
        set => this.SetValue(ref _hintsVisible, value);
    }
}
