using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class In
{
    private readonly ComputeInstance _instance = new();

    [TestMethod]
    public void In_Collection_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("nums", new List<int> { 1, 2, 3 });
        _instance.AddExpression("a", "2 in nums");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_Collection_MissingValue_ReturnsFalse()
    {
        _instance.Variables.Add("nums", new List<int> { 1, 2, 3 });
        _instance.AddExpression("a", "5 in nums");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_Collection_WithStringList_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("words", new List<string> { "hello", "world" });
        _instance.AddExpression("a", "\"hello\" in words");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_Collection_WithStringList_MissingValue_ReturnsFalse()
    {
        _instance.Variables.Add("words", new List<string> { "hello", "world" });
        _instance.AddExpression("a", "\"missing\" in words");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("x", 2);
        _instance.AddExpression("a", "x in (1, 2, 3)");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_MissingValue_ReturnsFalse()
    {
        _instance.Variables.Add("x", 5);
        _instance.AddExpression("a", "x in (1, 2, 3)");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_SingleElement_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("x", 42);
        _instance.AddExpression("a", "x in (42)");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithStringLiterals_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("s", "hello");
        _instance.AddExpression("a", "s in (\"hello\", \"world\")");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithStringLiterals_MissingValue_ReturnsFalse()
    {
        _instance.Variables.Add("s", "missing");
        _instance.AddExpression("a", "s in (\"hello\", \"world\")");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithLiteralOperand_ReturnsTrue()
    {
        _instance.AddExpression("a", "2 in (1, 2, 3)");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithLiteralOperand_ReturnsFalse()
    {
        _instance.AddExpression("a", "5 in (1, 2, 3)");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }
}
