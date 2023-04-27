using System;

namespace Slithin.VPL.NodeBuilding
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
