using Slithin.Scripting.Parsing;

namespace Slithin.Scripting.Interactive;

public static class Program
{
    public static void Main()
    {
        var interpreter = new Execution.Interpreter();
        interpreter.Variables.Add("today", DateTime.Today.DayOfWeek);

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            var tree = Parser.Parse(new SourceDocument("interactive", input));

            interpreter.Messages.Clear();

            var result = tree.Tree.Accept(interpreter);

            foreach (var msg in tree.Messages)
            {
                Console.WriteLine(msg);
            }

            foreach (var msg in interpreter.Messages)
            {
                Console.WriteLine(msg);
            }

            Console.WriteLine(result);
        }
    }
}
