using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class In
{
    private readonly ComputeInstance _instance = new();

    [TestMethod]
    public void In_List_ContainsValue_ReturnsTrue()
    {
        _instance.AddExpression("a", "3 in (1, 2, 3)");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_MissingValue_ReturnsFalse()
    {
        _instance.AddExpression("a", "5 in (1, 2, 3)");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithVariable_ContainsValue_ReturnsTrue()
    {
        _instance.Variables.Add("x", 2);
        _instance.AddExpression("a", "x in (1, 2, 3)");
        Assert.IsTrue(_instance.GetResult<bool>("a"));
    }

    [TestMethod]
    public void In_List_WithVariable_MissingValue_ReturnsFalse()
    {
        _instance.Variables.Add("x", 9);
        _instance.AddExpression("a", "x in (1, 2, 3)");
        Assert.IsFalse(_instance.GetResult<bool>("a"));
    }

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
}
