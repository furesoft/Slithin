using System;

namespace Slithin.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ImportProviderBaseTypeAttribute : Attribute
    {
        public ImportProviderBaseTypeAttribute(string extension)
        {
            Extension = extension;
        }

        public string Extension { get; set; }
    }
}
