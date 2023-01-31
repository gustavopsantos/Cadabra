# Cadabra

## Benchmark Sample Overview

**NOTE: All methods marked with `Benchmark` will have benchmark code injected.**

Before code:
```csharp
class MyClass
{
    [Benchmark]
    void MyMethod()
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(120));
    }
}
```

What gets compiled:
```csharp	
class MyClass
{
    [Benchmark]
    void MyMethod()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Thread.Sleep(TimeSpan.FromMilliseconds(120));
        Debug.Log($"MyClass::MyMethod took {stopwatch.ElapsedMilliseconds}ms.");
    }
}
```
> The actual injected class and method names are different

## Credits

Cadabra was made possible by:
- [com.unity.entities](https://github.com/needle-mirror/com.unity.entities)
