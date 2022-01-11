using System.Collections.Generic;
using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.Serializer;
using Slithin.Core;
using Slithin.UI;

namespace Slithin.ViewModels;

public class VplWindowViewModal : BaseViewModel
{
    private readonly NodeFactory _factory;

    private readonly INodeSerializer _serializer;
    private IDrawingNode? _drawing;
    private IList<INodeTemplate>? _templates;

    public VplWindowViewModal()
    {
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

    public IList<INodeTemplate>? Templates
    {
        get => _templates;
        set => SetValue(ref _templates, value);
    }
}
