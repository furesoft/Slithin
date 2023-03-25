using Avalonia.Media;

namespace Slithin.Modules.Bot;

public class BitmapMessage : Syn.Bot.Oscova.Messages.Message
{
    public IImage Bitmap { get; set; }

    public BitmapMessage(IImage bitmap) : base("image")
    {
        Bitmap = bitmap;
    }
}
