using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Slithin.Core;

namespace Slithin.Controls
{
    public class CommandTextBox : TemplatedControl
    {
        public static StyledProperty<ICommand> ActionProperty =
            AvaloniaProperty.Register<CommandTextBox, ICommand>("ActionParameter");

        public static StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<CommandTextBox, ICommand>("Command");

        public static StyledProperty<string> CommandTextProperty =
            AvaloniaProperty.Register<CommandTextBox, string>("CommandTextProperty");

        public static StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<CommandTextBox, string>("Text");

        public static StyledProperty<string> WatermarkProperty =
                                            AvaloniaProperty.Register<CommandTextBox, string>("Watermark");

        public CommandTextBox()
        {
            Action = new DelegateCommand(DoAction);
        }

        public ICommand Action
        {
            get { return GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public ICommand Command
        {
            get { return GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public string CommandText
        {
            get { return GetValue(CommandTextProperty); }
            set { SetValue(CommandTextProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        private void DoAction(object obj)
        {
            Command.Execute(Text);
            Text = "";
        }
    }
}
