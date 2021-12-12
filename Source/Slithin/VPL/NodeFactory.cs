using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NodeEditor.Model;
using NodeEditor.ViewModels;
using Slithin.VPL;
using Slithin.VPL.Components.ViewModels;
using Slithin.VPL.NodeBuilding;

namespace Slithin.UI;

public class NodeFactory
{
    public static NodeViewModel CreateViewModel(VisualNode vm, double x, double y, double width, double height)
    {
        var node = new NodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = vm
        };

        return node;
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

        var entry = CreateEntry(drawing.Width / 5, drawing.Height / 3);
        var exit = CreateExit(drawing.Width / 5 * 4, drawing.Height / 3);

        entry.Parent = drawing;
        exit.Parent = drawing;

        drawing.Nodes.Add(entry);
        drawing.Nodes.Add(exit);

        return drawing;
    }

    public INode CreateEntry(double x, double y, double width = 60, double height = 60, double pinSize = 8)
    {
        return CreateNode(new EntryNode(), x, y, width, height, pinSize);
    }

    public INode CreateExit(double x, double y, double width = 60, double height = 60, double pinSize = 8)
    {
        return CreateNode(new ExitNode(), x, y, width, height, pinSize);
    }

    public INode CreateNode(VisualNode vm, double x, double y, double width = 120, double height = 60, double pinSize = 8)
    {
        var node = CreateViewModel(vm, x, y, width, height);
        var pins = vm.GetType().GetProperties()
            .Where(_ => _.GetCustomAttribute<PinAttribute>() != null)
            .Select(_ => (_.GetCustomAttribute<PinAttribute>(), _));

        var inputPins = pins.Where(_ => _.Item2.PropertyType == typeof(IInputPin)).ToArray();
        var outputPins = pins.Where(_ => _.Item2.PropertyType == typeof(IOutputPin)).ToArray();

        for (int i = 0; i < inputPins.Length; i++)
        {
            var pin = inputPins[i];

            node.AddPin(0, height * 1.0 / (inputPins.Length + 1) * (i + 1), pinSize, pinSize,
                pin.Item1.Alignment != PinAlignment.None ? pin.Item1.Alignment : PinAlignment.Left,
                pin.Item1.Name ?? pin._.Name);
        }

        for (int i = 0; i < outputPins.Length; i++)
        {
            var pin = outputPins[i];

            node.AddPin(width, height * 1.0 / (outputPins.Length + 1) * (i + 1), pinSize, pinSize,
                 pin.Item1.Alignment != PinAlignment.None ? pin.Item1.Alignment : PinAlignment.Right,
                 pin.Item1.Name ?? pin._.Name);
        }

        return node;
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        var templates = new ObservableCollection<INodeTemplate>();

        var nodes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(VisualNode).IsAssignableFrom(x) && x.IsClass)
            .Where(x => x.Name != nameof(VisualNode))
            .Select(type => (VisualNode)Activator.CreateInstance(type));

        foreach (var node in nodes)
        {
            var ignoreAttribute = node.GetType().GetCustomAttribute<IgnoreTemplateAttribute>();
            if (ignoreAttribute != null)
            {
                continue;
            }

            templates.Add(new NodeTemplateViewModel
            {
                Title = node.Label,
                Build = (x, y) => CreateNode(node, x, y),
                Preview = CreateNode(node, 0, 0)
            });
        }

        return templates;
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
                Debug.WriteLine($"{start.Parent?.Content.GetType().Name}:{start.GetType().Name} -> {end.Parent?.Content.GetType().Name}:{end.GetType().Name}");
            }
        }
    }
}
