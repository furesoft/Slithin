using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Bot.Controls.Chat;

public partial class ChatControl : UserControl
{
    public ChatControl()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        ((ObservableCollection<ChatMessage>)this.FindControl<ItemsPresenter>("chatList").Items).CollectionChanged += OnCollectionChanged;
        this.FindControl<ItemsPresenter>("chatList").ItemContainerGenerator.Materialized += ItemContainerGeneratorOnMaterialized;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.FindControl<ScrollViewer>("chatScrollViewer").ScrollToEnd();
    }

    private void ItemContainerGeneratorOnMaterialized(object? sender, ItemContainerEventArgs e)
    {
       this.FindControl<ScrollViewer>("chatScrollViewer").ScrollToEnd();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
