using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Slithin.Core;

namespace Slithin.Controls;

public class KryptoDonateControl : TemplatedControl
{
    public static StyledProperty<string> AddressProperty = AvaloniaProperty.Register<KryptoDonateControl, string>(nameof(Address), "0000000000000000000000");
    public static StyledProperty<string> CoinNameProperty = AvaloniaProperty.Register<KryptoDonateControl, string>(nameof(CoinName), "0000000000000000000000");
    public static StyledProperty<ICommand> CopyAddressCommandProperty = AvaloniaProperty.Register<KryptoDonateControl, ICommand>(nameof(CopyAddressCommand));
    public static StyledProperty<IImage> ImageProperty = AvaloniaProperty.Register<KryptoDonateControl, IImage>(nameof(Image));

    public KryptoDonateControl()
    {
        CopyAddressCommand = new DelegateCommand(CopyAddress);
    }

    public string Address
    {
        get { return GetValue(AddressProperty); }
        set { SetValue(AddressProperty, value); }
    }

    public string CoinName
    {
        get { return GetValue(CoinNameProperty); }
        set { SetValue(CoinNameProperty, value); }
    }

    public ICommand CopyAddressCommand
    {
        get { return GetValue(CopyAddressCommandProperty); }
        set { SetValue(CopyAddressCommandProperty, value); }
    }

    public IImage Image
    {
        get { return GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    private async void CopyAddress(object obj)
    {
        await Application.Current.Clipboard.SetTextAsync(Address);
    }
}
