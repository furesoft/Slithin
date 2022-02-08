namespace Slithin.Scripting.Execution;

public class BindingTable
{
    public Dictionary<string, object> Variables { get; set; } = new();

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
            return Variables[name];
        }

        return null;
    }

    public bool IsCallable(string name)
    {
        if (IsVariable(name))
        {
            return GetVariable(name) is ICallable;
        }

        return false;
    }

    public bool IsVariable(string name)
    {
        return Variables.ContainsKey(name);
    }
}
