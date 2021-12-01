using System.Collections.Generic;
using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.ViewModels;
using Slithin.VPL.Components.ViewModels;

namespace Slithin.UI;

public class NodeFactory
{
    public INode CreateOrGate(double x, double y, double width = 60, double height = 60, int count = 1,
        double pinSize = 8)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new OrGateViewModel {Label = "≥", Count = count}
        };
        //((IDrawingNode)node.Parent).Connectors[0];
        node.AddPin(0, 2, pinSize, pinSize, PinAlignment.Left);
        node.AddPin(0, height - 2, pinSize, pinSize, PinAlignment.Left);
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right);

        return node;
    }

    public IConnector CreateConnector(IPin? start, IPin? end)
    {
        return new ConnectorViewModel {Start = start, End = end};
    }

    public IDrawingNode CreateDrawing()
    {
        var drawing = new DrawingNodeViewModel
        {
            X = 0,
            Y = 0,
            Width = 900,
            Height = 600,
            Nodes = new ObservableCollection<INode>(),
            Connectors = new ObservableCollection<IConnector>()
        };

        return drawing;
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        return new ObservableCollection<INodeTemplate>
        {
            new NodeTemplateViewModel {Title = "Or", Build = (x, y) => CreateOrGate(x, y)}
        };
    }
}
