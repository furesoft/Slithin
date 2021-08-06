using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NiL.JS;
using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Extensions;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Core.Scripting.Extensions;

namespace Slithin.Core.Scripting
{
    public static class Automation
    {
        public static void CreateNotbook(string template, int pageCount)
        {
            var document = new PdfDocument
            {
                PageLayout = PdfPageLayout.SinglePage,
                PageMode = PdfPageMode.FullScreen
            };

            var p = document.AddPage();
            var pgfx = XGraphics.FromPdfPage(p);
            pgfx.DrawString("Notebook", new XFont("Arial", 50), XBrushes.Black, new XPoint(150, 150));

            var t = XImage.FromFile(ServiceLocator.TemplatesDir + "\\" + template + ".png");

            p.AddDocumentLink(new PdfRectangle(new XPoint(0, 50), new XSize(10, 10)), 1);
            pgfx.DrawString($"Page 1", new XFont("Arial", 20), XBrushes.Black, new XPoint(10, 50));

            for (var i = 0; i < pageCount; i++)
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(t, 0, 0);
            }

            document.Save(ServiceLocator.ConfigBaseDir + "\\test.pdf");
        }

        public static void Evaluate(string scriptname)
        {
            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts", scriptname + ".js");

            if (File.Exists(path))
            {
                var mainModule = new Module($"Scripts/{scriptname}.js", File.ReadAllText(path));
                mainModule.Context.DefineVariable("events").Assign(JSValue.Wrap(ServiceLocator.Events));
                mainModule.Context.DefineVariable("config", false).Assign(Utils.ToJSObject((JObject)GetScriptInfo(scriptname).Config));
                mainModule.Context.DefineVariable("saveConfig").Assign(new ExternalFunction((t, args) =>
                {
                    var confObj = Utils.ConvertToJObject(args.First().Value.As<JSObject>());

                    GetScriptInfo(scriptname).Save(confObj);
                    return null;
                }));

                mainModule.ModuleResolversChain.Add(new ModuleResolver());

                mainModule.Run();
            }
        }

        public static ScriptInfo GetScriptInfo(string scriptName)
        {
            var file = Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts", scriptName + ".info");
            return JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(file));
        }

        public static IEnumerable<ScriptInfo> GetScriptInfos()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts"), "*.info"))
            {
                yield return JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(file));
            }
        }

        public static string[] GetScriptNames()
        {
            return Directory.GetFiles(
                    Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts")).
                    Select(_ => Path.GetFileNameWithoutExtension(_)).
                    ToArray();
        }

        public static void Init()
        {
            Parser.DefineCustomCodeFragment(typeof(UsingStatement));
            Parser.DefineCustomCodeFragment(typeof(OnCallStatement));

            Parser.DefineCustomCodeFragment(typeof(KeysOfOperator));
        }
    }
}
