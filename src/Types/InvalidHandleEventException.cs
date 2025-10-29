using System.Reflection;

namespace KExtensions.Types;

/// <summary>
/// Represents errors that occur during WeakEventManager.HandleEvent execution.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="T:InvalidHandleEventException"/> class.
/// </remarks>
/// <param name="message">Message.</param>
/// <param name="targetParameterCountException">Target parameter count exception.</param>
public class InvalidHandleEventException(string message, TargetParameterCountException targetParameterCountException) : Exception(message, targetParameterCountException)
{
}