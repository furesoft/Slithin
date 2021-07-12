using System.IO;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;

namespace Slithin.Core
{
    public static class Automation
    {
        public static Context CreateContext()
        {
            var c = new Context();

            c.DefineVariable("lib").Assign(new NamespaceProvider("System"));

            return c;
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
