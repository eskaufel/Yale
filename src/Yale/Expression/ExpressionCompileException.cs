using Yale.Parser;
using Yale.Resources;

namespace Yale.Expression;

public sealed class ExpressionCompileException : Exception
{
    internal ExpressionCompileException(string message, CompileExceptionReason reason)
        : base(message) => Reason = reason;

    internal ExpressionCompileException(ParserLogException parseException)
        : base(string.Empty, parseException) => Reason = CompileExceptionReason.SyntaxError;

    public override string Message
    {
        get
        {
            if (Reason is CompileExceptionReason.SyntaxError)
            {
                return $"{CompileErrors.SyntaxError}: {InnerException?.Message}";
            }

            return base.Message;
        }
    }

    /// <summary>
    /// Explains the reason why compilation failed.
    /// </summary>
    public CompileExceptionReason Reason { get; }
}
