using System.Collections.Generic;
using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.Serializer;
using Slithin.Core;
using Slithin.UI;
using Slithin.VPL;

namespace Slithin.ViewModels;

public class VplWindowViewModal : BaseViewModel
{
    private readonly NodeFactory _factory;

    private readonly INodeSerializer _serializer;
    private IList<NodeCategory>? _categories;
    private IDrawingNode? _drawing;

    public VplWindowViewModal()
    {
        _serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _factory = new NodeFactory();

        _categories = _factory.CreateTemplates();

        Drawing = _factory.CreateDrawing();
        Drawing.Serializer = _serializer;
    }

    public IList<NodeCategory>? Categories
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
