using System.Linq;

namespace Slithin.Core.Remarkable
{
    public static class FolderManager
    {
        private static Folder root;

        public static Metadata[] GetChildren(Metadata folder)
        {
            return null;
        }
    }

    public class Document : INode
    {
        public Metadata Value { get; set; }
    }

    public class Folder : INode
    {
        public INode[] Children { get; set; }
    }

    public abstract class INode
    {
        public Folder Parent { get; set; }
    }
}
