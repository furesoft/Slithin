namespace Slithin.Scripting.Execution;

public class DelegateCallable : ICallable
{
    public DelegateCallable(Delegate handler)
    {
        Handler = handler;
    }

    public Delegate Handler { get; set; }

    public object Invoke(object[] args)
    {
        return Handler.DynamicInvoke(args);
    }
}
