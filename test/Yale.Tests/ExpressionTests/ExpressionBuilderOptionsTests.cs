using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;
using Yale.Expression;

namespace Yale.Tests.ExpressionTests;

[TestClass]
public class ExpressionBuilderOptionsTests
{
    private ComputeInstance _instance = new();

    [TestMethod]
    public void DefaultValues()
    {
        _instance.AddExpression("a", int.MaxValue.ToString());

        //OverflowCheck
        Assert.ThrowsException<OverflowException>(() => _instance.AddExpression("b", "a + 1"));
        _instance.AddExpression("b", "a - 1");

        //IntegerAsDouble (Default == false)
        Assert.AreEqual(typeof(int), _instance.GetResult("a").GetType());

        #region CaseSensitive

        //Variables
        Assert.ThrowsException<ExpressionCompileException>(() => _instance.AddExpression("c", "A"));
        _instance.AddExpression("c", "a");

        //Expression
        Assert.ThrowsException<ExpressionCompileException>(() => _instance.AddExpression("d", "C"));
        _instance.AddExpression("d", "c");

        //Members
        _instance.Variables.Add("rand", new Random());
        Assert.ThrowsException<ExpressionCompileException>(
            () => _instance.AddExpression("e", "rand.nextDouble() + 100")
        );
        _instance.AddExpression("e", "rand.NextDouble() + 100");

        #endregion CaseSensitive

        //StringComparison
        _instance.AddExpression("f", "\"hello\" = \"Hello\"");
        Assert.IsFalse(_instance.GetResult<bool>("f"));

        _instance.AddExpression("g", "\"hello\" = \"hello\"");
        Assert.IsTrue(_instance.GetResult<bool>("g"));

        //ReadLiteral
        _instance.AddExpression("h", "1.0");
        Assert.AreEqual(typeof(double), _instance.GetResult("h").GetType());
    }

    [TestMethod]
    public void RealLiteralDataTypeTest()
    {
        _instance = new ComputeInstance(
            new ComputeInstanceOptions()
            {
                ExpressionOptions = new ExpressionBuilderOptions
                {
                    RealLiteralDataType = RealLiteralDataType.Decimal
                }
            }
        );
        _instance.AddExpression("a", "4.0");
        Assert.AreEqual(typeof(decimal), _instance.GetResult("a").GetType());

        _instance = new ComputeInstance(
            new ComputeInstanceOptions()
            {
                ExpressionOptions = new ExpressionBuilderOptions
                {
                    RealLiteralDataType = RealLiteralDataType.Single
                }
            }
        );
        _instance.AddExpression("a", "4.0");
        Assert.AreEqual(typeof(Single), _instance.GetResult("a").GetType());
    }

    [TestMethod]
    public void IntegerAsDouble_TreatsIntegerLiteralsAsDouble()
    {
        _instance = new ComputeInstance(
            new ComputeInstanceOptions
            {
                ExpressionOptions = new ExpressionBuilderOptions { IntegerAsDouble = true }
            }
        );
        _instance.AddExpression("a", "42");
        Assert.AreEqual(typeof(double), _instance.GetResult("a").GetType());
        Assert.AreEqual(42.0, _instance.GetResult<double>("a"));
    }

    [TestMethod]
    public void CaseSensitive_False_AllowsCaseInsensitiveMemberAccess()
    {
        _instance = new ComputeInstance(
            new ComputeInstanceOptions
            {
                ExpressionOptions = new ExpressionBuilderOptions { CaseSensitive = false }
            }
        );
        _instance.Variables.Add("rand", new Random());
        // With case-insensitive mode, lowercase method name resolves to NextDouble
        _instance.AddExpression("a", "rand.nextdouble() + 0");
        Assert.IsInstanceOfType(_instance.GetResult<double>("a"), typeof(double));
    }

    [TestMethod]
    public void CaseSensitive_False_AllowsCaseInsensitiveExpressionReference()
    {
        _instance = new ComputeInstance(
            new ComputeInstanceOptions
            {
                ExpressionOptions = new ExpressionBuilderOptions { CaseSensitive = false }
            }
        );
        // nameNodeMap uses OrdinalIgnoreCase when CaseSensitive=false,
        // so expression key "result" is reachable as "RESULT" in another expression
        _instance.AddExpression("result", "5 + 3");
        _instance.AddExpression("doubled", "RESULT * 2");
        Assert.AreEqual(16, _instance.GetResult<int>("doubled"));
    }
}
