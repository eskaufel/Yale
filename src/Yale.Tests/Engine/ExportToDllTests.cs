using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yale.Engine;

namespace Yale.Tests.Engine;

[TestClass]
public class ExportToDllTests
{
    private static string TempDllPath(string name) =>
        Path.Combine(Path.GetTempPath(), $"Yale.Export.{name}.dll");

    [TestMethod]
    public void ExportToDll_CreatesFileOnDisk()
    {
        var path = TempDllPath(nameof(ExportToDll_CreatesFileOnDisk));
        try
        {
            var instance = new ComputeInstance();
            instance.AddExpression<int>("add", "1 + 2");
            instance.ExportToDll(path);

            Assert.IsTrue(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestMethod]
    public void ExportToDll_GeneratedAssemblyLoads()
    {
        var path = TempDllPath(nameof(ExportToDll_GeneratedAssemblyLoads));
        try
        {
            var instance = new ComputeInstance();
            instance.AddExpression<int>("add", "1 + 2");
            instance.ExportToDll(path);

            var assembly = Assembly.LoadFrom(path);
            Assert.IsNotNull(assembly);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestMethod]
    public void ExportToDll_GeneratedTypeContainsMethodPerExpression()
    {
        var path = TempDllPath(nameof(ExportToDll_GeneratedTypeContainsMethodPerExpression));
        try
        {
            var instance = new ComputeInstance();
            instance.AddExpression<int>("exprA", "1 + 2");
            instance.AddExpression<bool>("exprB", "true");
            instance.ExportToDll(path);

            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("YaleGeneratedExpressions");
            Assert.IsNotNull(type, "YaleGeneratedExpressions type not found");

            var methodA = type.GetMethod("exprA");
            Assert.IsNotNull(methodA, "Method exprA not found");

            var methodB = type.GetMethod("exprB");
            Assert.IsNotNull(methodB, "Method exprB not found");
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestMethod]
    public void ExportToDll_MethodHasCorrectReturnType()
    {
        var path = TempDllPath(nameof(ExportToDll_MethodHasCorrectReturnType));
        try
        {
            var instance = new ComputeInstance();
            instance.AddExpression<double>("calc", "3.14 * 2.0");
            instance.ExportToDll(path);

            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("YaleGeneratedExpressions")!;
            var method = type.GetMethod("calc")!;

            Assert.AreEqual(typeof(double), method.ReturnType);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestMethod]
    public void ExportToDll_MethodHasThreeParameters()
    {
        var path = TempDllPath(nameof(ExportToDll_MethodHasThreeParameters));
        try
        {
            var instance = new ComputeInstance();
            instance.AddExpression<int>("expr", "42");
            instance.ExportToDll(path);

            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("YaleGeneratedExpressions")!;
            var method = type.GetMethod("expr")!;

            Assert.AreEqual(3, method.GetParameters().Length);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [TestMethod]
    public void ExportToDll_NullOutputPath_Throws()
    {
        var instance = new ComputeInstance();
        instance.AddExpression<int>("x", "1");

        Assert.ThrowsException<ArgumentNullException>(() => instance.ExportToDll(null!));
    }

    [TestMethod]
    public void ExportToDll_EmptyInstance_CreatesEmptyType()
    {
        var path = TempDllPath(nameof(ExportToDll_EmptyInstance_CreatesEmptyType));
        try
        {
            var instance = new ComputeInstance();
            instance.ExportToDll(path);

            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("YaleGeneratedExpressions")!;
            Assert.AreEqual(
                0,
                type.GetMethods(
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
                ).Length
            );
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
