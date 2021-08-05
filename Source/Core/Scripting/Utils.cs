using Newtonsoft.Json.Linq;
using NiL.JS.Core;
using NiL.JS.Extensions;
using Slithin.Core;

namespace Slithin.Core.Scripting
{
    public static class Utils
    {
        public static JObject ConvertToJObject(JSObject obj)
        {
            JObject o = new JObject();

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
