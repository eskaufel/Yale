using BenchmarkDotNet.Attributes;
using Yale.Engine;

namespace Yale.Benchmarks.Parse;

/// <summary>
/// Measures the cost of parsing and compiling expression strings into evaluable delegates.
/// Each iteration creates a fresh ComputeInstance so prior state does not carry over.
/// </summary>
[MemoryDiagnoser]
public class ParseBenchmarks
{
    [Benchmark]
    public ComputeInstance IntegerLiteral()
    {
        var instance = new ComputeInstance();
        instance.AddExpression<int>("expr", "42");
        return instance;
    }

    [Benchmark]
    public ComputeInstance ArithmeticExpression()
    {
        var instance = new ComputeInstance();
        instance.Variables.Add("a", 3.0);
        instance.Variables.Add("b", 4.0);
        instance.AddExpression<double>("expr", "a * 2.0 + b / 1.5");
        return instance;
    }

    [Benchmark]
    public ComputeInstance BooleanExpression()
    {
        var instance = new ComputeInstance();
        instance.Variables.Add("a", 3.0);
        instance.Variables.Add("b", 4.0);
        instance.AddExpression<bool>("expr", "a > 1.0 AND b < 10.0");
        return instance;
    }

    [Benchmark]
    public ComputeInstance StringLiteral()
    {
        var instance = new ComputeInstance();
        instance.AddExpression<string>("expr", "\"hello world\"");
        return instance;
    }
}
