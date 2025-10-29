using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using KExtensions;
using KExtensions.Tasks;
using NUnit.Framework;

namespace KExtensions.Test;

[TestFixture]
public class TaskExtensionsTests
{
    [Test]
    public void Forget_DoesNotThrow_OnFaultedTask()
    {
        var t = Task.FromException(new InvalidOperationException("boom"));
        // Should not throw synchronously
        Assert.DoesNotThrow(() => t.Forget());
    }

    [Test]
    public Task SafeFireAndForget_Invokes_OnException_Callback()
    {
        Exception? captured = null;
        var t = Task.Run(() => throw new ApplicationException("fail"));
        t.SafeFireAndForget(returnToCallingContext: false, onException: ex => captured = ex);
        Assert.That(captured, Is.InstanceOf<ApplicationException>());
        Assert.That(captured!.Message, Is.EqualTo("fail"));
        return Task.CompletedTask;
    }

    [Test]
    public async Task SplitIntoBlocks_Success_GoesToResult()
    {
        var task = Task.FromResult(123);
        var (result, exception) = await task.SplitIntoBlocks();
        // result block should have value and be completed, exception block should not have value
        Assert.That(result.TryReceive(out var value), Is.True);
        Assert.That(value, Is.EqualTo(123));
        Assert.That(exception.TryReceive(out _), Is.False);
    }

    [Test]
    public async Task SplitIntoBlocks_Exception_GoesToException()
    {
        var task = Task.FromException<int>(new InvalidOperationException("x"));
        var (result, exception) = await task.SplitIntoBlocks();
        Assert.That(result.TryReceive(out _), Is.False);
        Assert.That(exception.TryReceive(out var ex), Is.True);
        Assert.That(ex, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.Message, Is.EqualTo("x"));
    }
}
