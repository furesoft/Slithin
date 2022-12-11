using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace Slithin.Themes.Generic.Styles
{
    public class RadioButtonGroup : ItemsControl
    {
        public static readonly StyledProperty<string> GroupNameProperty =
            AvaloniaProperty.Register<RadioButtonGroup, string>(nameof(GroupName));

        public static readonly StyledProperty<Orientation> OrientationProperty =
            AvaloniaProperty.Register<RadioButtonGroup, Orientation>(nameof(Orientation), Orientation.Horizontal);

        public string GroupName
        {
            get => GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);

            foreach (var item in Items)
            {
                if (item is RadioButton radioButton)
                {
                    radioButton.GroupName = GroupName;
                }
            }
        }
    }
}