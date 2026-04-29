namespace Yale.Engine.Interface;

internal interface IExpressionResult
{
    string Name { get; }

    string ExpressionText { get; }

    object? ResultAsObject { get; }

    Type? ResultType { get; }

    Type DeclaredType { get; }

    void Recalculate();

    /// <summary>
    /// ExpressionResult is dirty when any of the expressions or variables used in the expression has changed
    /// and the result has not been recalculated
    /// </summary>
    bool Dirty { get; set; }
}
