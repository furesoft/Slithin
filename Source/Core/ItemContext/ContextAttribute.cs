using System;

namespace Slithin.Core.ItemContext
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContextAttribute : Attribute
    {
        public ContextAttribute(UIContext context)
        {
            Context = context;
        }

        public UIContext Context { get; set; }
    }
}
