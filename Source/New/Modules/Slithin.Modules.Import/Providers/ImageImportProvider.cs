using SkiaSharp;
using Slithin.Modules.Import.Models;

namespace Slithin.ImportProviders;

[ImportProviderBaseType(".png")]
public class ImageImportProvider : IImportProvider
{
    public bool CanHandle(string filename)
    {
        var extension = Path.GetExtension(filename);

        return extension == ".jpg" || extension == ".bmp" || extension == ".gif" || extension == ".tiff";
    }

    public Stream Import(Stream source)
    {
        var outputStrm = new MemoryStream();
        using var skBitmap = SKBitmap.Decode(source);
        using var image = SKImage.FromBitmap(skBitmap);

        image.Encode(SKEncodedImageFormat.Png, 100)
            .SaveTo(outputStrm);

        outputStrm.Seek(0, SeekOrigin.Begin);

        return outputStrm;
    }
}
