using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.BaseServices;

public class KeyBindingImpl : IKeyBinding
{
    private readonly Dictionary<KeyGesture, ICommand> _gestures = new();
    public void Register(string gesture, ICommand command)
    {
        _gestures.Add(KeyGesture.Parse(gesture), command);
    }

    public void Init(Window window)
    {
        foreach (var gesture in _gestures)
        {
            window.KeyBindings.Add(new(){ Gesture = gesture.Key, Command = gesture.Value });
        }
    }
}
