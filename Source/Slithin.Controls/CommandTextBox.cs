﻿using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Slithin.Core.MVVM;

namespace Slithin.Controls;

public class CommandTextBox : TemplatedControl
{
    public static readonly StyledProperty<ICommand> ActionProperty =
        AvaloniaProperty.Register<CommandTextBox, ICommand>("ActionParameter");

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<CommandTextBox, ICommand>("Command");

    public static readonly StyledProperty<string> CommandTextProperty =
        AvaloniaProperty.Register<CommandTextBox, string>("CommandTextProperty");

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<CommandTextBox, string>("Text");

    public static readonly StyledProperty<string> WatermarkProperty =
        AvaloniaProperty.Register<CommandTextBox, string>("Watermark");

    public CommandTextBox()
    {
        Action = new DelegateCommand(DoAction);
    }

    public ICommand Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public string CommandText
    {
        get => GetValue(CommandTextProperty);
        set => SetValue(CommandTextProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    private void DoAction(object obj)
    {
        Command.Execute(Text);
        Text = "";
    }
}
