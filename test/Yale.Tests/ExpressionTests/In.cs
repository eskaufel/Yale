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
}
