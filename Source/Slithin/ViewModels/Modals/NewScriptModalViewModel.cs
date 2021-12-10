using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NodeEditor.Model;
using NodeEditor.Serializer;
using Slithin.Core;
using Slithin.UI;
using Slithin.VPL;

namespace Slithin.ViewModels.Modals;

public class NewScriptModalViewModel : BaseViewModel
{
    private readonly NodeFactory _factory;

    private readonly INodeSerializer _serializer;
    private IDrawingNode? _drawing;
    private bool _isEditMode = true;
    private string _name;

    private string _selectedCategory;
    private int _step;
    private IList<INodeTemplate>? _templates;

    public NewScriptModalViewModel()
    {
        NextCommand = new DelegateCommand(Next);

        _serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _factory = new NodeFactory();

        _templates = _factory.CreateTemplates();

        Drawing = _factory.CreateDrawing();
        Drawing.Serializer = _serializer;
    }

    public IDrawingNode? Drawing
    {
        get => _drawing;
        set => SetValue(ref _drawing, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetValue(ref _isEditMode, value);
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

    public IList<INodeTemplate>? Templates
    {
        get => _templates;
        set => SetValue(ref _templates, value);
    }

    private void Next(object obj)
    {
        if (Step == 0)
        {
            Step++;

            var window = new VplWindow();
            window.Show();
        }

        _factory.PrintNetList(Drawing);
    }
}
