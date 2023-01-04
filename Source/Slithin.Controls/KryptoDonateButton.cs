using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;
using SkiaSharp.QrCode.Image;
using Slithin.Core.MVVM;

namespace Slithin.Controls;

public class KryptoDonateButton : TemplatedControl
{
    public static StyledProperty<string> AddressProperty = AvaloniaProperty.Register<KryptoDonateButton, string>(nameof(Address), "0000000000000000000000");
    public static StyledProperty<string> CoinNameProperty = AvaloniaProperty.Register<KryptoDonateButton, string>(nameof(CoinName), "0000000000000000000000");
    public static StyledProperty<ICommand> CopyAddressCommandProperty = AvaloniaProperty.Register<KryptoDonateButton, ICommand>(nameof(CopyAddressCommand));
    public static StyledProperty<Drawing> ImageProperty = AvaloniaProperty.Register<KryptoDonateButton, Drawing>(nameof(Image));
    public static StyledProperty<IImage> QrProperty = AvaloniaProperty.Register<KryptoDonateButton, IImage>(nameof(Qr));

    public KryptoDonateButton()
    {
        CopyAddressCommand = new DelegateCommand(CopyAddress);
    }

    public string Address
    {
        get { return GetValue(AddressProperty); }
        set
        {
            SetValue(AddressProperty, value);
            RegenerateQrCode(value);
        }
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

    public Drawing Image
    {
        get { return GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    public IImage Qr
    {
        get { return GetValue(QrProperty); }
        set { SetValue(QrProperty, value); }
    }

    private async void CopyAddress(object obj)
    {
        await Application.Current.Clipboard.SetTextAsync(Address);
    }

    private void RegenerateQrCode(string addr)
    {
        var qrCode = new QrCode(addr, new Vector2Slim(256, 256), SKEncodedImageFormat.Png);
        var output = new MemoryStream();
        qrCode.GenerateImage(output);

        output.Seek(0, SeekOrigin.Begin);

        Qr = new Bitmap(output);
    }
}
