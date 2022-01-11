namespace Slithin.ModuleSystem.StdLib;

//ToDo: Solve Allocator Problem: Every Module Should Have A Unique Allocator
public class Allocator
{
    public static int HeapBaseAddress;

    private static readonly LinkedList<FreeNode> _freeList = new();
    private static readonly LinkedList<FreeNode> _allocationList = new();

    public static int Allocate(int size)
    {
        var node = FindFreeNode(size);

        return node.Address + HeapBaseAddress;
    }

    private static FreeNode FindFreeNode(int size)
    {
        if (_freeList.Count == 0)
        {
            var n = new FreeNode {Size = size, Address = 0};
            _allocationList.AddLast(n);

            return n;
        }

        var node = _freeList.First;
        do
        {
            if (node.Value.Size <= size)
            {
                _freeList.Remove(node);
                _allocationList.AddLast(node);

                return node.Value;
            }

            node = node.Next;
        } while (node.Next != null);

        return new FreeNode {Size = size, Address = node.Value.Address + 1};
    }

    public static void Free(int addr)
    {
        foreach (var freeNode in _allocationList)
            if (freeNode.Address == addr - HeapBaseAddress)
            {
                _freeList.AddLast(freeNode);
                _allocationList.Remove(freeNode);

                break;
            }
    }

    public static int AllocateString(int length)
    {
        //allocate normal memory
        //register address for automatic scope freeing
        //length is with null termination
        return 0;
    }
}

public struct FreeNode
{
    public int Address;
    public int Size;
}