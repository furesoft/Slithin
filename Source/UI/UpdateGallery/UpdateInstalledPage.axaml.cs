using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Services;

namespace Slithin.UI.UpdateGallery
{
    public partial class UpdateInstalledPage : UserControl
    {
        public UpdateInstalledPage()
        {
            var versionService = ServiceLocator.Container.Resolve<IVersionService>();
            Title = "Release " + versionService.GetLocalVersion().ToString();

            InitializeComponent();
        }

        public string Title { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = this;
        }
    }
}
