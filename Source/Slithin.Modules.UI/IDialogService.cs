using Avalonia.Controls;

namespace Slithin.Modules.UI.Models;

public interface IDialogService
{
    Task<bool> Show(string title, Control content);
}
