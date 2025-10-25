namespace KExtensions;

public static class DelegateExtensions
{
    /// <summary>
    /// Safe invoke multicast delegate
    /// Ever wanted to call a multicast delegate but you want the entire invocation list to be called even if an exception
    /// occurs in any in the chain. Then you are in luck, I have created an extension method that does just that, throwing an 
    /// AggregateException only after execution of the entire list completes:
    /// </summary>
    /// <param name="del"></param>
    /// <param name="args"></param>
    /// <exception cref="AggregateException"></exception>
    public static void SafeInvoke(this Delegate? del, params object[] args)
    {
        if (del == null) return;
        var exceptions = new List<Exception>();
        foreach (var handler in del.GetInvocationList())
        {
            try
            {
                handler.Method.Invoke(handler.Target, args);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Count != 0)
        {
            throw new AggregateException(exceptions);
        }
    }
    
    public static void SafeInvoke(this Action? action)
    {
        if (action == null) return;

        var exceptions = new List<Exception>();
        foreach (var handler in action.GetInvocationList())
        {
            try
            {
                ((Action)handler)();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Count != 0)
            throw new AggregateException(exceptions);
    }
}