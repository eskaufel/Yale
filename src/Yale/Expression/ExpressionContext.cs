using Yale.Core;
using Yale.Engine;

namespace Yale.Expression;

internal sealed class ExpressionContext
{
    public ExpressionContext(
        ExpressionBuilderOptions builderOptions,
        string expressionName,
        object owner,
        ImportCollection imports,
        VariableCollection variables,
        ComputeInstance computeInstance
    )
    {
        BuilderOptions = builderOptions;
        ExpressionName = expressionName;
        Owner = owner;
        OwnerType = owner?.GetType();
        Imports = imports;
        Variables = variables;
        ComputeInstance = computeInstance;
    }

    public object Owner { get; }

    public Type? OwnerType { get; }

    internal ExpressionBuilderOptions BuilderOptions { get; } = new ExpressionBuilderOptions();

    public ImportCollection Imports { get; set; }

    public VariableCollection Variables { get; set; }

    public ComputeInstance ComputeInstance { get; set; }

    public string ExpressionName { get; }
}
