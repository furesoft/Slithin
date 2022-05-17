namespace Slithin.Core;

public static class Debouncer
{
    //Used for delayed binding
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
        var last = 0;
        return arg =>
        {
            var current = Interlocked.Increment(ref last);
            Task.Delay(milliseconds).ContinueWith(task =>
            {
                if (current == last) func(arg);
                task.Dispose();
            });
        };
    }
}
