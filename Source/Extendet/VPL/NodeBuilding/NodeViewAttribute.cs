﻿using System;

namespace Slithin.VPL.NodeBuilding
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeViewAttribute : Attribute
    {
        public NodeViewAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}
