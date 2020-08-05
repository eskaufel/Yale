using System;
using System.Globalization;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;
using Yale.Resources;

namespace Yale.Expression.Elements.Literals.Integral
{
    internal class UInt64LiteralElement : IntegralLiteralElement
    {
        private readonly UInt64 value;

        public UInt64LiteralElement(string image, NumberStyles ns)
        {
            try
            {
                value = UInt64.Parse(image, ns, CultureInfo.InvariantCulture);
            }
            catch (OverflowException)
            {
                throw CreateCompileException(CompileErrors.ValueNotRepresentableInType, CompileExceptionReason.ConstantOverflow, image, ResultType.Name);
            }
        }

        public UInt64LiteralElement(UInt64 value)
        {
            this.value = value;
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            EmitLoad(Convert.ToInt64(value), ilGenerator);
        }

        public override Type ResultType => typeof(UInt64);
    }
}