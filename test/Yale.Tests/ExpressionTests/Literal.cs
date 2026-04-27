using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;
using Yale.Expression;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class Literal
{
    private readonly ComputeInstance _instance = new();

    [TestMethod]
    public void Char_StandaloneValue()
    {
        _instance.AddExpression("a", "'A'");
        Assert.AreEqual('A', _instance.GetResult<char>("a"));
    }

    [TestMethod]
    public void Char_EqualityTrue()
    {
        _instance.AddExpression("a", "'A' = 'A'");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void Char_EqualityFalse()
    {
        _instance.AddExpression("a", "'A' = 'B'");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void DateTime_LiteralValue()
    {
        _instance.AddExpression("a", "#01/05/2023#");
        Assert.AreEqual(new DateTime(2023, 5, 1), _instance.GetResult<DateTime>("a"));
    }

    [TestMethod]
    public void DateTime_EqualityTrue()
    {
        _instance.AddExpression("a", "#01/05/2023# = #01/05/2023#");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void DateTime_EqualityFalse()
    {
        _instance.AddExpression("a", "#01/05/2023# = #02/05/2023#");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void DateTime_CustomFormat()
    {
        var instance = new ComputeInstance(
            new ComputeInstanceOptions
            {
                ExpressionOptions = new ExpressionBuilderOptions { DateTimeFormat = "yyyy-MM-dd" }
            }
        );
        instance.AddExpression("a", "#2023-05-01#");
        Assert.AreEqual(new DateTime(2023, 5, 1), instance.GetResult<DateTime>("a"));
    }

    [TestMethod]
    public void TimeSpan_LiteralValue()
    {
        _instance.AddExpression("a", "##02:30:00#");
        Assert.AreEqual(new TimeSpan(2, 30, 0), _instance.GetResult<TimeSpan>("a"));
    }

    [TestMethod]
    public void TimeSpan_EqualityTrue()
    {
        _instance.AddExpression("a", "##02:30:00# = ##02:30:00#");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void TimeSpan_WithDays()
    {
        _instance.AddExpression("a", "##1.02:30:00#");
        Assert.AreEqual(new TimeSpan(1, 2, 30, 0), _instance.GetResult<TimeSpan>("a"));
    }
}
