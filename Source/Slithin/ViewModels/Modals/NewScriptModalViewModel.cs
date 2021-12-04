using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NodeEditor.Controls;
using NodeEditor.Model;
using NodeEditor.Serializer;
using Slithin.Core;
using Slithin.UI;

namespace Slithin.ViewModels.Modals;

public class NewScriptModalViewModel : BaseViewModel
{
    private readonly NodeFactory _factory;

    private readonly INodeSerializer _serializer;
    private IDrawingNode? _drawing;
    private string _name;

    private string _selectedCategory;
    private int _step;
    private IList<INodeTemplate>? _templates;

    private bool _isEditMode = true;

    public NewScriptModalViewModel()
    {
        NextCommand = new DelegateCommand(Next);

        _serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _factory = new NodeFactory();

        _templates = _factory.CreateTemplates();

        Drawing = _factory.CreateDrawing();
        Drawing.Serializer = _serializer;
    }

    public IList<INodeTemplate>? Templates
    {
        get => _templates;
        set => SetValue(ref _templates, value);
    }

    public IDrawingNode? Drawing
    {
        get => _drawing;
        set => SetValue(ref _drawing, value);
    }

    public ICommand NextCommand { get; set; }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetValue(ref _selectedCategory, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetValue(ref _isEditMode, value);
    }

    public int Step
    {
        get => _step;
        set => SetValue(ref _step, value);
    }

    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }


    private void Next(object obj)
    {
        if (Step == 0)
        {
            Step++;
        }

        
        _factory.PrintNetList(Drawing);
    }
}
