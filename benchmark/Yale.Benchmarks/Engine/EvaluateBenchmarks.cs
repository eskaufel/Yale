using BenchmarkDotNet.Attributes;
using Yale.Engine;

namespace Yale.Benchmarks.Engine;

/// <summary>
/// Measures the cost of evaluating already-compiled expressions via GetResult.
/// Setup compiles all expressions once; each benchmark iteration only calls GetResult.
/// </summary>
[MemoryDiagnoser]
public class EvaluateBenchmarks
{
    private ComputeInstance _intInstance = null!;
    private ComputeInstance _arithmeticInstance = null!;
    private ComputeInstance _boolInstance = null!;
    private ComputeInstance _chainedInstance = null!;

    [GlobalSetup]
    public void Setup()
    {
        _intInstance = new ComputeInstance();
        _intInstance.AddExpression<int>("result", "42");

        _arithmeticInstance = new ComputeInstance();
        _arithmeticInstance.Variables.Add("a", 3.0);
        _arithmeticInstance.Variables.Add("b", 4.0);
        _arithmeticInstance.AddExpression<double>("result", "a * 2.0 + b / 1.5");

        _boolInstance = new ComputeInstance();
        _boolInstance.Variables.Add("a", 3.0);
        _boolInstance.Variables.Add("b", 4.0);
        _boolInstance.AddExpression<bool>("result", "a > 1.0 AND b < 10.0");

        _chainedInstance = new ComputeInstance();
        _chainedInstance.Variables.Add("x", 5);
        _chainedInstance.AddExpression<int>("doubled", "x * 2");
        _chainedInstance.AddExpression<int>("result", "doubled + 1");
    }

    [Benchmark]
    public object IntegerLiteral() => _intInstance.GetResult("result");

    [Benchmark]
    public object ArithmeticExpression() => _arithmeticInstance.GetResult("result");

    [Benchmark]
    public object BooleanExpression() => _boolInstance.GetResult("result");

    [Benchmark]
    public object ChainedExpressions() => _chainedInstance.GetResult("result");
}
