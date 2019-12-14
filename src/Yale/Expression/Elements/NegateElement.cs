﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

using Yale.Expression.Elements.Base;
using Yale.Parser.Internal;
using Yale.Resources;

namespace Yale.Expression.Elements
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "<Pending>")]
    internal class NegateElement : UnaryElement
    {
        private const string UnaryNegoation = "UnaryNegation";

        public override Type ResultType { get; }

        public NegateElement(ExpressionElement child) : base(child)
        {
            ResultType = GetResultType(child.ResultType);
        }

        protected override Type GetResultType(Type childType)
        {
            var typeCode = Type.GetTypeCode(childType);

            var methodInfo = Utility.GetSimpleOverloadedOperator(UnaryNegoation, childType, childType);
            if (methodInfo != null)
            {
                return methodInfo.ReturnType;
            }

            switch (typeCode)
            {
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return childType;

                case TypeCode.UInt32:
                    return typeof(Int64);

                default:
                    throw CompileException(CompileErrors.OperationNotDefinedForType, CompileExceptionReason.TypeMismatch, MyChild.ResultType.Name);
            }
        }

        public override void Emit(YaleIlGenerator ilGenerator, ExpressionContext context)
        {
            var resultType = ResultType;
            MyChild.Emit(ilGenerator, context);
            ImplicitConverter.EmitImplicitConvert(MyChild.ResultType, resultType, ilGenerator);

            var methodInfo = Utility.GetSimpleOverloadedOperator(UnaryNegoation, resultType, resultType);

            if (methodInfo == null)
            {
                ilGenerator.Emit(OpCodes.Neg);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Call, methodInfo);
            }
        }
    }
}