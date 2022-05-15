using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Slithin.Controls.Ports.Panels;

//ported from https://github.com/HandyOrg/HandyControl.git

/// <summary>
/// 'itemscontrol' which use a panel to add it's children
/// </summary>
public class SimpleItemsControl : TemplatedControl
{
    /// <summary>
    /// Defines the HasItems property.
    /// </summary>
    public static readonly StyledProperty<bool> HasItemsProperty =
    AvaloniaProperty.Register<SimpleItemsControl, bool>(nameof(HasItems));

    /// <summary>
    /// Defines the ItemsSource property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
    AvaloniaProperty.Register<SimpleItemsControl, IEnumerable>(nameof(ItemsSource));

    /// <summary>
    /// Defines the ItemTemplate property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
    AvaloniaProperty.Register<SimpleItemsControl, IDataTemplate>(nameof(ItemTemplate));

    private const string ElementPanel = "PART_Panel";

    public SimpleItemsControl()
    {
        ItemTemplateProperty.Changed.AddClassHandler<SimpleItemsControl>((o, e) => OnItemTemplateChanged(o, e));
        ItemsSourceProperty.Changed.AddClassHandler<SimpleItemsControl>((o, e) => OnItemsSourceChanged(o, e));

        var items = new AvaloniaList<object>();
        items.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                SetValue(HasItemsProperty, true);
            }
            OnItemsChanged(s, e);
        };
        Items = items;
    }

    /// <summary>
    /// Gets or sets HasItems.
    /// </summary>
    public bool HasItems
    {
        get { return GetValue(HasItemsProperty); }
        private set { SetValue(HasItemsProperty, value); }
    }

    public IEnumerable Items { get; internal set; }

    /// <summary>
    /// Gets or sets ItemsSource.
    /// </summary>
    public IEnumerable ItemsSource
    {
        get { return GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Gets or sets ItemTemplate.
    /// </summary>
    [Bindable(true)]
    public IDataTemplate ItemTemplate
    {
        get { return GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    internal IPanel ItemsHost { get; set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        ItemsHost?.Children.Clear();
        base.OnApplyTemplate(e);

        ItemsHost = e.NameScope.Find<IPanel>(ElementPanel);
        Refresh();
    }

    protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
    }

    protected virtual void OnItemTemplateChanged(SimpleItemsControl o, AvaloniaPropertyChangedEventArgs e)
    {
        Refresh();
    }

    protected virtual void Refresh()
    {
        if (ItemsHost == null)
            return;

        ItemsHost.Children.Clear();
        foreach (var item in Items)
        {
            if (item is TemplatedControl element)
            {
                //element.Style = ItemContainerStyle;
                ItemsHost.Children.Add(element);
            }
        }
    }

    protected virtual void UpdateItems()
    {
    }

    private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Refresh();
        UpdateItems();
    }

    private void OnItemsSourceChanged(SimpleItemsControl o, AvaloniaPropertyChangedEventArgs e)
    {
        o.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
    }
}
