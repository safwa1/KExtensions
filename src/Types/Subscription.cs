using System.Reflection;

namespace KExtensions.Types;

public readonly struct Subscription(in WeakReference? subscriber, in MethodInfo handler) : IEquatable<Subscription>
{
	public WeakReference? Subscriber { get; } = subscriber;
	public MethodInfo Handler { get; } = handler ?? throw new ArgumentNullException(nameof(handler));

	public bool Equals(Subscription other)
	{
		return Equals(Subscriber, other.Subscriber) && Handler.Equals(other.Handler);
	}

	public override bool Equals(object? obj)
	{
		return obj is Subscription other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Subscriber, Handler);
	}
    public static bool operator ==(Subscription left, Subscription right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Subscription left, Subscription right)
    {
        return !(left == right);
    }
}