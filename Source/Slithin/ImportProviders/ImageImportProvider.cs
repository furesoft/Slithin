using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Slithin.Core.ImportExport;

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
        var img = Image.FromStream(source);
        var outputStrm = new MemoryStream();

        img.Save(outputStrm, ImageFormat.Png);

        outputStrm.Seek(0, SeekOrigin.Begin);

        return outputStrm;
    }
}
