using System;

namespace Slithin.Core.VPLNodeBuilding
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeCategoryAttribute : Attribute
    {
        public NodeCategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; set; }
    }
}
