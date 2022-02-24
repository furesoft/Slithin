namespace Slithin.Scripting.Execution;

public class BindingTable
{
    private Dictionary<string, string> Aliases { get; set; } = new();
    private Dictionary<string, object> Variables { get; set; } = new();

    public void AddAlias(string oldName, string newName)
    {
        if (!Aliases.ContainsKey(oldName))
        {
            Aliases.Add(newName, oldName);
        }
    }

    public void AddVariable(string name, object value)
    {
        if (!Variables.ContainsKey(name))
        {
            Variables.Add(name, value);
        }
        else
        {
            Variables[name] = value;
        }
    }

    public object Call(string name, object[] arguments)
    {
        if (IsVariable(name))
        {
            var value = Variables[name];

            if (value is ICallable callable)
            {
                return callable.Invoke(arguments);
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    public object GetVariable(string name)
    {
        if (IsVariable(name))
        {
            if (Aliases.ContainsKey(name))
            {
                name = Aliases[name];
            }

            return Variables[name];
        }

        return null;
    }

    public IEnumerable<string> GetVariableNames()
    {
        return Variables.Keys;
    }

    public bool IsCallable(string name)
    {
        if (Aliases.ContainsKey(name))
        {
            name = Aliases[name];
        }

        if (IsVariable(name))
        {
            return GetVariable(name) is ICallable;
        }

        return false;
    }

    public bool IsVariable(string name)
    {
        return Variables.ContainsKey(name) || Aliases.ContainsKey(name);
    }
}
