using System.IO;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using QRCoder;
using Slithin.Core;

namespace Slithin.Controls;

public class KryptoDonateControl : TemplatedControl
{
    public static StyledProperty<string> AddressProperty = AvaloniaProperty.Register<KryptoDonateControl, string>(nameof(Address), "0000000000000000000000");
    public static StyledProperty<string> CoinNameProperty = AvaloniaProperty.Register<KryptoDonateControl, string>(nameof(CoinName), "0000000000000000000000");
    public static StyledProperty<ICommand> CopyAddressCommandProperty = AvaloniaProperty.Register<KryptoDonateControl, ICommand>(nameof(CopyAddressCommand));
    public static StyledProperty<IImage> ImageProperty = AvaloniaProperty.Register<KryptoDonateControl, IImage>(nameof(Image));
    public static StyledProperty<IImage> QrProperty = AvaloniaProperty.Register<KryptoDonateControl, IImage>(nameof(Qr));

    public KryptoDonateControl()
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

    public IImage Image
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
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(addr, QRCodeGenerator.ECCLevel.L);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        Qr = new Bitmap(new MemoryStream(qrCodeImage));
    }
}
