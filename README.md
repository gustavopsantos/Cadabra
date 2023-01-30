# Unity IL Weaver

## Benchmark Sample Overview

**NOTE: All methods marked with `Benchmark` will have benchmark code injected.**

Before code:
```csharp
[Benchmark]
private static void Foo()
{
    Thread.Sleep(TimeSpan.FromMilliseconds(50));
}
```

What gets compiled:
```csharp	[Benchmark]
private static void Foo()
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    Thread.Sleep(TimeSpan.FromMilliseconds(50.0));
    Debug.Log($"Benchy::Foo took {stopwatch.ElapsedMilliseconds}ms.");
}
```
> The actual injected class and method names are different
