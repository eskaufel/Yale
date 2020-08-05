using System;
using System.Globalization;
using Yale.Expression.Elements.Base.Literals;
using Yale.Parser.Internal;

namespace Yale.Expression.Elements.Literals.Integral
{
    internal class Int64LiteralElement : IntegralLiteralElement
    {
        private Int64 value;
        private const string MinValue = "9223372036854775808";
        private readonly bool isMinValue;

        public Int64LiteralElement(Int64 value)
        {
            this.value = value;
        }

        private Int64LiteralElement()
        {
            isMinValue = true;
        }

        public static Int64LiteralElement? TryCreate(string image, bool isHex, bool negated)
        {
            if (negated & image == MinValue)
            {
                return new Int64LiteralElement();
            }

            if (isHex)
            {
                if (Int64.TryParse(image, NumberStyles.AllowHexSpecifier, null, out var value) == false)
                {
                    return null;
                }

                //Todo: What does this do?
                if (value >= 0 & value <= Int64.MaxValue)
                {
                    return new Int64LiteralElement(value);
                }

                return null;
            }
            else
            {
                if (Int64.TryParse(image, out var value))
                {
                    return new Int64LiteralElement(value);
                }
                else
                {
                    return null;
                }
            }
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            EmitLoad(value, ilGenerator);
        }

        public void Negate()
        {
            if (isMinValue)
            {
                value = Int64.MinValue;
            }
            else
            {
                value = -value;
            }
        }

        public override Type ResultType => typeof(Int64);
    }
}