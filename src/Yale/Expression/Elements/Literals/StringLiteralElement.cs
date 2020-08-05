using System;
using System.Reflection.Emit;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;

namespace Yale.Expression.Elements.Literals
{
    internal class StringLiteralElement : LiteralElement
    {
        private readonly string value;

        public StringLiteralElement(string value)
        {
            this.value = value;
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            ilGenerator.Emit(OpCodes.Ldstr, value);
        }

        public override Type ResultType => typeof(string);
    }
}