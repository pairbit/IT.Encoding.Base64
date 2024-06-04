# IT.Collections.Factory
[![NuGet version (IT.Collections.Factory)](https://img.shields.io/nuget/v/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)
[![NuGet pre version (IT.Collections.Factory)](https://img.shields.io/nuget/vpre/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)

Implementation of collections factory

## Register factories

```csharp
var registry = new EnumerableFactoryRegistry();
//var registry = new ConcurrentEnumerableFactoryRegistry();
registry.RegisterFactoriesDefault();
```

## New empty list

```csharp
var listFactory = registry.GetFactory<ListFactory>();
var list = listFactory.Empty<int>();
Assert.That(list.Capacity, Is.EqualTo(0));
```

## New array and new list with capacity

```csharp
var arrayFactory = registry.GetFactory<ArrayFactory>();
Assert.That(arrayFactory.Kind.IsFixed(), Is.True);
Assert.That(listFactory.Kind.IsFixed(), Is.False);

var array = arrayFactory.New<int>(3);
Assert.That(array.Length, Is.EqualTo(3));

list = listFactory.New<int>(4);
Assert.That(list.Capacity, Is.EqualTo(4));
Assert.That(list.Count, Is.EqualTo(0));
```

## New LinkedList with IgnoreCapacity

```csharp
var linkedListFactory = registry.GetFactory<LinkedListFactory>();

Assert.That(linkedListFactory.Kind.IsIgnoreCapacity(), Is.True);

var linkedList = linkedListFactory.New<int>(-1, tryAdd =>
{
    tryAdd(1);
    tryAdd(2);
    tryAdd(3);
});

Assert.That(linkedList.SequenceEqual(new[] { 1, 2, 3 }), Is.True);
```

## New IReadOnlySet with comparer

```csharp
#if NET6_0_OR_GREATER
    var roSetFactory = registry.GetFactory<IReadOnlySetFactory>();
    Assert.That(roSetFactory.Kind.IsUnique(), Is.True);
    Assert.That(roSetFactory.Kind.IsEquatable(), Is.True);

    var roSet = roSetFactory.New(2, tryAdd =>
    {
        Assert.That(tryAdd("Test"), Is.True);
        Assert.That(tryAdd("tEsT"), Is.False);
    }, StringComparer.OrdinalIgnoreCase.ToComparers());
    Assert.That(roSet.Count, Is.EqualTo(1));
#endif
```

## New Stack from generic factory 

```csharp
var intStackFactory = registry.GetFactory<Stack<int>, int>();

Assert.That(intStackFactory.Kind.IsProxy(), Is.True);
Assert.That(intStackFactory.Kind.IsReverse(), Is.True);

var stack = intStackFactory.New(3, tryAdd =>
{
    tryAdd(3);
    tryAdd(2);
    tryAdd(1);
});

Assert.That(stack.SequenceEqual(new[] { 1, 2, 3 }), Is.True);
```

## New collections with builder

```csharp
IEnumerable<int> data = Enumerable.Range(5, 10);

//EnumerableKind: None
CheckFactory(data, registry.GetFactory<ListFactory>());

//EnumerableKind: IgnoreCapacity
CheckFactory(data, registry.GetFactory<LinkedListFactory>());

//EnumerableKind: Reverse
CheckFactory(data, registry.GetFactory<StackFactory>());

//EnumerableKind: IgnoreCapacity, Reverse, ThreadSafe
CheckFactory(data, registry.GetFactory<ConcurrentBagFactory>());

static void CheckFactory<T>(IEnumerable<T> data, IEnumerableFactory factory)
{
    IEnumerable<T> newEnumerable;

    var kind = factory.Kind;

    if (kind.IsThreadSafe())
    {
        var capacity = -1;

        if (!kind.IsIgnoreCapacity())
        {
            //allocation, need use ArrayPool
            var dataArray = data.ToArray();

            capacity = dataArray.Length;

            data = dataArray;
        }

        newEnumerable = factory.New<T, IEnumerable<T>>(capacity, BuildParallel, in data);

        Assert.That(newEnumerable.OrderBy(x => x).SequenceEqual(data), Is.True);
    }

    if (kind.IsIgnoreCapacity() && !kind.IsReverse())
    {
        newEnumerable = factory.New(-1, static (TryAdd<T> tryAdd, in IEnumerable<T> data) =>
        {
            foreach (var i in data) tryAdd(i);
        }, in data);
    }
    else
    {
        //allocation, need use ArrayPool
        var dataArray = data.ToArray();

        newEnumerable = factory.New<T, T[]>(dataArray.Length, 
            kind.IsReverse() ? BuildReverse : Build, in dataArray);
    }

    Assert.That(newEnumerable.SequenceEqual(data), Is.True);
}

static void Build<T>(TryAdd<T> tryAdd, in T[] data)
{
    for (int i = 0; i < data.Length; i++) tryAdd(data[i]);
}

static void BuildReverse<T>(TryAdd<T> tryAdd, in T[] data)
{
    for (int i = data.Length - 1; i >= 0; i--) tryAdd(data[i]);
}

static void BuildParallel<T>(TryAdd<T> tryAdd, in IEnumerable<T> data)
{
    Parallel.ForEach(data, i => tryAdd(i));
}
```
