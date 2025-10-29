using KExtensions.Generic;
using NUnit.Framework;

namespace KExtensions.Test;

[TestFixture]
public class AnyExtensionsTests
{
    private sealed class Box { public int V; }

    [Test]
    public void Run_Let_Apply_Also_Work()
    {
        var b = new Box { V = 1 }
            .Apply(x => x.V = 2)
            .Also(x => x.V += 3);
        Assert.That(b.V, Is.EqualTo(5));

        var res = b.Run(x => x.V * 2);
        Assert.That(res, Is.EqualTo(10));

        var res2 = b.Let(x => x.V.ToString());
        Assert.That(res2, Is.EqualTo("5"));
    }

    [Test]
    public void TakeIf_TakeUnless()
    {
        string s = "abc";
        Assert.That(AnyExtensions.TakeIf(s, x => x.Length == 3), Is.EqualTo("abc"));
        Assert.That(AnyExtensions.TakeIf(s, x => x.Length == 5), Is.Null);
        Assert.That(AnyExtensions.TakeUnless(s, x => x.StartsWith("ab")), Is.Null);
        Assert.That(AnyExtensions.TakeUnless(s, x => x.StartsWith("zz")), Is.EqualTo("abc"));
    }
}
