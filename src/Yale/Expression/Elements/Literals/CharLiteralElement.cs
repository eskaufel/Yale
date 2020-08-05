using System;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;

namespace Yale.Expression.Elements.Literals
{
    internal class CharLiteralElement : LiteralElement
    {
        private readonly char value;

        public CharLiteralElement(char value)
        {
            this.value = value;
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            var intValue = Convert.ToInt32(value);
            EmitLoad(intValue, ilGenerator);
        }

        public override Type ResultType => typeof(char);
    }
}