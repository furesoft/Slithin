using System.ComponentModel;
using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualDropDownButton : ContextualButton
{
    public Control DropDown { get; set; }

    public ContextualDropDownButton(string title, string iconName, Control dropdown) : base(title, iconName, null)
    {
        DropDown = dropdown;

        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DataContext))
        {
            DropDown.DataContext = DataContext;
        }
    }
}
