using System;

namespace Slithin.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnabledAttribute : Attribute
    {
        public EnabledAttribute(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; set; }
    }
}
