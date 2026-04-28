using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class FunctionCall
{
    private readonly ComputeInstance _instance = new();

    [TestMethod]
    public void TwoArgInstanceMethod_LiteralArgs_ReturnsExpected()
    {
        // Random.Next(minValue, maxValue) returns minValue when the range is a single value
        _instance.Variables.Add("rand", new Random());
        _instance.AddExpression<int>("e", "rand.Next(5, 6)");
        Assert.AreEqual(5, _instance.GetResult<int>("e"));
    }

    [TestMethod]
    public void TwoArgInstanceMethod_VariableArgs_ReturnsExpected()
    {
        _instance.Variables.Add("rand", new Random());
        _instance.Variables.Add("lo", 10);
        _instance.Variables.Add("hi", 11);
        _instance.AddExpression<int>("e", "rand.Next(lo, hi)");
        Assert.AreEqual(10, _instance.GetResult<int>("e"));
    }

    [TestMethod]
    public void TwoArgInstanceMethod_StringSubstring_ReturnsExpected()
    {
        _instance.Variables.Add("s", "hello world");
        _instance.AddExpression<string>("e", "s.Substring(6, 5)");
        Assert.AreEqual("world", _instance.GetResult<string>("e"));
    }

    [TestMethod]
    public void TwoArgStaticMethod_Imported_ReturnsMinimum()
    {
        _instance.Imports.AddType(typeof(Math));
        _instance.Variables.Add("a", 3);
        _instance.Variables.Add("b", 7);
        _instance.AddExpression<int>("e", "Min(a, b)");
        Assert.AreEqual(3, _instance.GetResult<int>("e"));
    }

    [TestMethod]
    public void TwoArgStaticMethod_Imported_ReturnsMaximum()
    {
        _instance.Imports.AddType(typeof(Math));
        _instance.Variables.Add("a", 3);
        _instance.Variables.Add("b", 7);
        _instance.AddExpression<int>("e", "Max(a, b)");
        Assert.AreEqual(7, _instance.GetResult<int>("e"));
    }

    [TestMethod]
    public void ThreeArgStaticMethod_Imported_ReturnsClampedValue()
    {
        _instance.Imports.AddType(typeof(Math));
        _instance.AddExpression<double>("e", "Clamp(5.0, 1.0, 10.0)");
        Assert.AreEqual(5.0, _instance.GetResult<double>("e"));
    }

    [TestMethod]
    public void TwoArgInstanceMethod_ResultUsedInExpression_ReturnsExpected()
    {
        _instance.Variables.Add("rand", new Random());
        _instance.AddExpression<int>("e", "rand.Next(3, 4) + rand.Next(7, 8)");
        Assert.AreEqual(10, _instance.GetResult<int>("e"));
    }
}
