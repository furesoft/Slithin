using Slithin.Scripting.Execution;
using Slithin.Scripting.Parsing;

namespace Slithin.Scripting.Interactive;

public static class Program
{
    public static void Main()
    {
        var interpreter = new Interpreter();
        interpreter.BindingTable.AddVariable("today", DateTime.Today.DayOfWeek);
        interpreter.BindingTable.AddVariable("show notification", new DelegateCallable(new Action<string>(Console.WriteLine)));
        interpreter.BindingTable.AddAlias("today", "heute");
        interpreter.BindingTable.AddVariable("environment", interpreter.BindingTable);
        interpreter.BindingTable.AddVariable("list elements of environment", new DelegateCallable(new Action(() =>
        {
            foreach (var binding in interpreter.BindingTable.GetVariableNames())
            {
                Console.WriteLine(binding + ": " + interpreter.BindingTable.GetVariable(binding));
            }
        }
        )));

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
