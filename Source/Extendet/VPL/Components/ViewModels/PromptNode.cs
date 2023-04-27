﻿using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(PromptView))]
public class PromptNode : VisualNode
{
    private object? _header;
    private object? _value;

    public PromptNode() : base("Show Prompt")
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowOutputPin { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Header
    {
        get => _header;
        set => SetValue(ref _header, value);
    }

    [Pin("Value")]
    public IOutputPin OutputValuePin { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }
}
