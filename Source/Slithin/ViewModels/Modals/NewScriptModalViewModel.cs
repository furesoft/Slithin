using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.MVVM;

namespace Slithin.ViewModels.Modals;

public class NewScriptModalViewModel : ModalBaseViewModel
{
    private string _name;

    private string _selectedCategory;
    private int _step;

    public NewScriptModalViewModel()
    {
        NextCommand = new DelegateCommand(Next);
    }

    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    public ICommand NextCommand { get; set; }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetValue(ref _selectedCategory, value);
    }

    public int Step
    {
        get => _step;
        set => SetValue(ref _step, value);
    }

    private void Next(object obj)
    {
        if (Step == 0)
        {
            Step++;
        }
    }
}
