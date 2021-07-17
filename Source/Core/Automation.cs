using System.IO;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Slithin.Core
{
    public static class Automation
    {
        public static Context CreateContext()
        {
            var c = new Context();

            c.DefineVariable("lib").Assign(new NamespaceProvider("System"));
            c.DefineVariable("pdf").Assign(new NamespaceProvider("PdfSharpCore.Pdf"));

            return c;
        }

        public static void CreateNotbook(string template, int pageCount)
        {
            var document = new PdfDocument();
            document.PageLayout = PdfPageLayout.SinglePage;
            document.PageMode = PdfPageMode.FullScreen;

            var p = document.AddPage();
            var pgfx = XGraphics.FromPdfPage(p);
            pgfx.DrawString("Notebook", new XFont("Arial", 50), XBrushes.Black, new XPoint(150, 150));

            var t = XImage.FromFile(ServiceLocator.TemplatesDir + "\\" + template + ".png");

            p.AddDocumentLink(new PdfRectangle(new XPoint(0, 50), new XSize(10, 10)), 1);
            pgfx.DrawString($"Page 1", new XFont("Arial", 20), XBrushes.Black, new XPoint(10, 50));

            for (int i = 0; i < pageCount; i++)
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(t, 0, 0);
            }

            document.Save(ServiceLocator.ConfigBaseDir + "\\test.pdf");
        }

        public static void Evaluate(string scriptname, Context context)
        {
            context.Eval(File.ReadAllText(Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts", scriptname + ".js")));
        }

        public static string[] GetScriptNames()
        {
            return Directory.GetFiles(
                    Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts")).
                    Select(_ => Path.GetFileNameWithoutExtension(_)).
                    ToArray();
        }
    }
}
