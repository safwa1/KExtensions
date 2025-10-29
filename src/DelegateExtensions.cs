using System.Reflection;

namespace KExtensions;

public static class DelegateExtensions
{
    private static void InvokeAll(IEnumerable<Delegate> handlers, Action<Delegate> invoker)
    {
        var exceptions = new List<Exception>();

        foreach (var handler in handlers)
        {
            try
            {
                invoker(handler);
            }
            catch (TargetInvocationException ex)
            {
                exceptions.Add(ex.InnerException ?? ex);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Count > 0)
            throw new AggregateException(exceptions);
    }

    /// <summary>
    /// Invokes all delegates in the multicast chain safely.
    /// Exceptions from individual handlers are collected and thrown as an AggregateException after completion.
    /// </summary>
    public static void SafeInvoke(this Delegate? self, params object[] args)
    {
        if (self == null) return;
        InvokeAll(self.GetInvocationList(), h => h.Method.Invoke(h.Target, args));
    }

    /// <summary>
    /// Similar to <see cref="SafeInvoke"/> but uses DynamicInvoke for more flexible (but slower) invocation.
    /// </summary>
    public static void SafeDynamicInvoke(this Delegate? self, params object[] args)
    {
        if (self == null) return;
        InvokeAll(self.GetInvocationList(), h => h.DynamicInvoke(args));
    }

    /// <summary>
    /// Specialized safe invocation for parameterless Action delegates.
    /// </summary>
    public static void SafeInvoke(this Action? self)
    {
        if (self == null) return;
        InvokeAll(self.GetInvocationList(), h => ((Action)h)());
    }

    /// <summary>
    /// Specialized safe invocation for single-argument Action delegates.
    /// </summary>
    public static void SafeInvoke<T>(this Action<T>? self, T arg)
    {
        if (self == null) return;
        InvokeAll(self.GetInvocationList(), h => ((Action<T>)h)(arg));
    }
}
