using System;
using NodeEditor.Model;

namespace Slithin.VPL.NodeBuilding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PinAttribute : Attribute
    {
        public PinAttribute(string name = null, PinAlignment alignment = PinAlignment.None)
        {
            Name = name;
            Alignment = alignment;
        }

        public PinAlignment Alignment { get; set; }
        public string Name { get; set; }
    }
}
