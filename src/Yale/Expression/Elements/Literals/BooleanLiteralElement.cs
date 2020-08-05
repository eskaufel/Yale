using System;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;

namespace Yale.Expression.Elements.Literals
{
    internal class BooleanLiteralElement : LiteralElement
    {
        private readonly bool value;

        public BooleanLiteralElement(bool value)
        {
            this.value = value;
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            EmitLoad(value, ilGenerator);
        }

        public override Type ResultType => typeof(bool);
    }
}