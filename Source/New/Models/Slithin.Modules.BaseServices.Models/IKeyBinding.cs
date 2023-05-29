using System.Windows.Input;
using Avalonia.Controls;

namespace Slithin.Modules.BaseServices.Models;

public interface IKeyBinding
{
    void Register(string gesture, ICommand command);
    void Init(Window window);
}
