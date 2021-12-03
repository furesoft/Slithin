using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NodeEditor.Model;
using NodeEditor.ViewModels;
using Slithin.VPL.Components.ViewModels;

namespace Slithin.UI;

public class NodeFactory
{
    public INode CreateEntry(double x, double y, double width = 60, double height = 60, double pinSize = 8)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new EntryViewModel { Label = "Entry" }
        };

        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right);

        return node;
    }

    public INode CreateExit(double x, double y, double width = 60, double height = 60, double pinSize = 8)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new ExitViewModel { Label = "Exit" }
        };

        node.AddPin(0, height / 4, pinSize, pinSize, PinAlignment.Left);
        node.AddPin(0, height / 4 * 3, pinSize, pinSize, PinAlignment.Left);

        return node;
    }

    public INode CreateShowNotification(double x, double y, double width = 120, double height = 60, double pinSize = 8)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new ShowNotificationViewModel { Label = "Show Notification" }
        };

        node.AddPin(0, height / 4, pinSize, pinSize, PinAlignment.Left);
        node.AddPin(0, height / 4 * 3, pinSize, pinSize, PinAlignment.Left);

        node.AddPin(width, height / 4, pinSize, pinSize, PinAlignment.Right);

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

        drawing.Nodes.Add(CreateEntry(drawing.Width / 5, drawing.Height / 3));
        drawing.Nodes.Add(CreateExit(drawing.Width / 5 * 4, drawing.Height / 3));

        return drawing;
    }

    public void PrintNetList(IDrawingNode? drawing)
    {
        if (drawing?.Connectors is null || drawing?.Nodes is null)
        {
            return;
        }

        foreach (var connector in drawing.Connectors)
        {
            if (connector.Start is { } start && connector.End is { } end)
            {
                Debug.WriteLine($"{start.Parent?.GetType().Name}:{start.GetType().Name} -> {end.Parent?.GetType().Name}:{end.GetType().Name}");
            }
        }
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        return new ObservableCollection<INodeTemplate>
        {
            new NodeTemplateViewModel {Title = "Show Notification", Build = (x, y) => CreateShowNotification(x, y)},
        };
    }
}
