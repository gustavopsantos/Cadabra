# Unity IL Weaver

## Important
After initializing unity for the first time, the weaver will be compiled at the same time as all other code.
So after having the first compilation, make any change to any code inside ILWeaver.Sample to force it to be recompiled, and from now on, weaved after each compilation.

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
