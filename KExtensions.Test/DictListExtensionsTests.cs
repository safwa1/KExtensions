using System;
using System.Collections.Generic;
using KExtensions;
using NUnit.Framework;

namespace KExtensions.Test;

[TestFixture]
public class DictListExtensionsTests
{
    [Test]
    public void AddAndRemoveFromCollection_Works_And_CleansUp_WhenEmpty()
    {
        var dict = new Dictionary<string, List<int>>();
        dict.AddToCollection("a", 1);
        dict.AddToCollection("a", 2);
        dict.AddToCollection("b", 5);

        Assert.That(dict["a"], Is.EquivalentTo(new[] {1,2}));
        Assert.That(dict["b"], Is.EquivalentTo(new[] {5}));

        var removed1 = dict.RemoveFromCollection("a", 1);
        Assert.That(removed1, Is.True);
        Assert.That(dict["a"], Is.EquivalentTo(new[] {2}));

        var removed2 = dict.RemoveFromCollection("a", 2);
        Assert.That(removed2, Is.True);
        Assert.That(dict.ContainsKey("a"), Is.False, "Key should be removed when collection is empty");

        var removed3 = dict.RemoveFromCollection("z", 0);
        Assert.That(removed3, Is.False);
    }

    [Test]
    public void CompareLists_Ordered_And_Unordered()
    {
        IReadOnlyList<int> a = new List<int> {1,2,3};
        IReadOnlyList<int> b = new List<int> {1,2,3};
        IReadOnlyList<int> c = new List<int> {3,2,1};
        IReadOnlyList<int> d = new List<int> {1,2,2};

        Assert.That(a.CompareLists(b, isOrdered:true), Is.True);
        Assert.That(a.CompareLists(c, isOrdered:true), Is.False);
        Assert.That(a.CompareLists(c, isOrdered:false), Is.True);
        Assert.That(a.CompareLists(d, isOrdered:false), Is.False);

        Assert.That(((IReadOnlyList<int>?)null).CompareLists(null, true), Is.True);
        Assert.That(((IReadOnlyList<int>?)null).CompareLists(new List<int>(), true), Is.False);
    }

    [Test]
    public void ContainsSequence_OnArrays()
    {
        var arr = new[] {1,2,3,4,5};
        Assert.That(arr.ContainsSequence(new[] {2,3,4}), Is.True);
        Assert.That(arr.ContainsSequence(new[] {3,5}), Is.False);
        Assert.That(arr.ContainsSequence(Array.Empty<int>()), Is.False);
        Assert.That(((int[]?)null).ContainsSequence(new[] {1}), Is.False);
    }

    [Test]
    public void NullableTypeHelpers()
    {
        Assert.That(((Type?)null).IsNullable(), Is.False);
        Assert.That(typeof(int?).IsNullable(), Is.True);
        Assert.That(typeof(int).IsNullable(), Is.False);

        Assert.That(typeof(int?).IsExactOrNullable<int>(), Is.True);
        Assert.That(typeof(int).IsExactOrNullable<int>(), Is.True);
        Assert.That(typeof(long).IsExactOrNullable<int>(), Is.False);

        var t = typeof(int).ToNullable();
        Assert.That(t, Is.EqualTo(typeof(int?)));
    }
}
