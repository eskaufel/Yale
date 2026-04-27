using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class Cast
{
    private readonly ComputeInstance _instance = new();

    [TestMethod]
    public void CastToInt()
    {
        _instance.AddExpression("cast", "cast(100.25; int)");

        Assert.AreEqual(100, _instance.GetResult("cast"));
    }

    [TestMethod]
    public void PowerFloatVariable()
    {
        _instance.Variables.Add("a", 4.0);
        _instance.AddExpression("b", "a^2");

        Assert.AreEqual(16.0, (double)_instance.GetResult("b"));
    }

    [TestMethod]
    public void CastToDouble()
    {
        _instance.AddExpression("a", "cast(100; double)");
        var result = _instance.GetResult("a");
        Assert.AreEqual(typeof(double), result.GetType());
        Assert.AreEqual(100.0, result);
    }

    [TestMethod]
    public void CastToByte()
    {
        _instance.AddExpression("a", "cast(200; byte)");
        var result = _instance.GetResult("a");
        Assert.AreEqual(typeof(byte), result.GetType());
        Assert.AreEqual((byte)200, result);
    }

    [TestMethod]
    public void CastToLong()
    {
        _instance.AddExpression("a", "cast(100; long)");
        var result = _instance.GetResult("a");
        Assert.AreEqual(typeof(long), result.GetType());
        Assert.AreEqual(100L, result);
    }

    [TestMethod]
    public void CastToShort()
    {
        _instance.AddExpression("a", "cast(32000; short)");
        var result = _instance.GetResult("a");
        Assert.AreEqual(typeof(short), result.GetType());
        Assert.AreEqual((short)32000, result);
    }
}
