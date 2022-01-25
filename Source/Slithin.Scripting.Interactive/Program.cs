using Slithin.Scripting.Parsing;

namespace Slithin.Scripting.Interactive;

public static class Program
{
    public static void Main()
    {
        var interpreter = new Execution.Interpreter();

        while (true)
        {
            var input = Console.ReadLine();
            var tree = Parser.Parse(new SourceDocument("interactive", input));

            var result = tree.Tree.Accept(interpreter);

            Console.WriteLine("> " + result);
        }
    }
}