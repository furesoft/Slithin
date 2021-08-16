using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Slithin.Core.Scripting
{
    public static class Utils
    {
        public static JObject ConvertToJObject(JSObject obj)
        {
            var o = new JObject();

            foreach (var item in obj)
            {
                if (item.Value.ValueType == JSValueType.Object)
                {
                    o.Add(item.Key, ConvertToJObject(item.Value.As<JSObject>()));
                }
                else
                {
                    o.Add(item.Key, JToken.FromObject(item.Value.Value));
                }
            }

            return o;
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public static JSObject ToJSObject(JObject obj)
        {
            var o = JSObject.CreateObject();
            foreach (var prop in obj)
            {
                if (prop.Value.Type == JTokenType.Object)
                {
                    o[prop.Key] = ToJSObject((JObject)prop.Value);
                }
                else
                {
                    o[prop.Key] = JSValue.Wrap(prop.Value.ToObject<object>());
                }
            }

            return o;
        }
    }
}
