using System.IO;
using Yale.Core;
using Yale.Engine.Interface;

namespace Yale.Expression;

internal sealed class AssemblyExpressionExporter
{
    private static readonly Type[] MethodParameterTypes =
    [
        typeof(object),
        typeof(ExpressionContext),
        typeof(VariableCollection),
    ];

    private readonly ExpressionBuilder _builder;

    internal AssemblyExpressionExporter(ExpressionBuilder builder) => _builder = builder;

    internal void Export(IEnumerable<IExpressionResult> expressions, string outputPath)
    {
        var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(outputPath));
        var assemblyBuilder = new PersistedAssemblyBuilder(assemblyName, typeof(object).Assembly);

        var module = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);

        // Allow the generated assembly to call Yale's internal types (e.g. ExpressionContext).
        DefineIgnoresAccessChecksTo(module, "Yale");

        var type = module.DefineType(
            "YaleGeneratedExpressions",
            TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class
        );

        foreach (var expr in expressions)
            EmitMethod(type, expr);

        type.CreateType();
        assemblyBuilder.Save(outputPath);
    }

    private void EmitMethod(TypeBuilder typeBuilder, IExpressionResult exprResult)
    {
        var method = typeBuilder.DefineMethod(
            exprResult.Name,
            MethodAttributes.Public | MethodAttributes.Static,
            exprResult.DeclaredType,
            MethodParameterTypes
        );

        method.DefineParameter(1, ParameterAttributes.None, "owner");
        method.DefineParameter(2, ParameterAttributes.None, "context");
        method.DefineParameter(3, ParameterAttributes.None, "variables");

        _builder.EmitToILGenerator(
            exprResult.Name,
            exprResult.ExpressionText,
            exprResult.DeclaredType,
            method.GetILGenerator()
        );
    }

    // The CLR recognises IgnoresAccessChecksToAttribute by its full name regardless of which
    // assembly defines it, so we emit the attribute type into the generated module itself.
    private static void DefineIgnoresAccessChecksTo(ModuleBuilder module, string targetAssembly)
    {
        var attrType = module.DefineType(
            "System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute",
            TypeAttributes.Public | TypeAttributes.Class,
            typeof(Attribute)
        );

        var ctor = attrType.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            [typeof(string)]
        );

        // Constructor body: call Attribute() then ret
        var ctorIl = ctor.GetILGenerator();
        ctorIl.Emit(OpCodes.Ldarg_0);
        ctorIl.Emit(OpCodes.Call, typeof(Attribute).GetConstructor(Type.EmptyTypes)!);
        ctorIl.Emit(OpCodes.Ret);

        var builtAttrType = attrType.CreateType();
        var attrCtor = builtAttrType.GetConstructor([typeof(string)])!;
        module.Assembly.SetCustomAttribute(
            new CustomAttributeBuilder(attrCtor, [targetAssembly])
        );
    }
}
