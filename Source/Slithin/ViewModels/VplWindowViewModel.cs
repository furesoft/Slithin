using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using NodeEditor.Model;
using NodeEditor.Serializer;
using Slithin.Core;
using Slithin.UI;

namespace Slithin.ViewModels;

public class VplWindowViewModal : BaseViewModel
{
    private readonly NodeFactory _factory;

    private readonly INodeSerializer _serializer;
    private IList<TabItem>? _categories;
    private IDrawingNode? _drawing;

    public VplWindowViewModal()
    {
        _serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _factory = new NodeFactory();

        _categories = _factory.CreateTemplates();

        Drawing = _factory.CreateDrawing();
        Drawing.Serializer = _serializer;
    }

    public IList<TabItem>? Categories
    {
        get => _categories;
        set => SetValue(ref _categories, value);
    }

    public IDrawingNode? Drawing
    {
        get => _drawing;
        set => SetValue(ref _drawing, value);
    }
}
