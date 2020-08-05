using System;
using System.Globalization;
using System.Reflection.Emit;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;
using Yale.Resources;

namespace Yale.Expression.Elements.Literals.Real
{
    internal class SingleLiteralElement : RealLiteralElement
    {
        private readonly float value;

        private SingleLiteralElement()
        { }

        public SingleLiteralElement(float value)
        {
            this.value = value;
        }

        public static SingleLiteralElement Parse(string image)
        {
            var element = new SingleLiteralElement();
            try
            {
                var value = float.Parse(image, CultureInfo.InvariantCulture);
                return new SingleLiteralElement(value);
            }
            catch (OverflowException)
            {
                throw element.CreateCompileException(CompileErrors.ValueNotRepresentableInType, CompileExceptionReason.ConstantOverflow, image, element.Name);
            }
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            ilGenerator.Emit(OpCodes.Ldc_R4, value);
        }

        public override Type ResultType => typeof(float);
    }
}