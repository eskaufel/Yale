﻿using System;
using System.Reflection.Emit;

using Yale.Parser.Internal;
using Yale.Resources;

namespace Yale.Expression.Elements.Base
{
    internal abstract class ExpressionElement
    {
        /// <summary>
        /// All expression elements must be able to emit their own Intermediate language
        /// </summary>
        /// <param name="ilGenerator"></param>
        /// <param name="context"></param>
        public abstract void Emit(YaleIlGenerator ilGenerator, ExpressionContext context);

        /// <summary>
        /// All expression elements must expose the Type they evaluate to
        /// </summary>
        public abstract Type ResultType { get; }

        public override string ToString()
        {
            return Name;
        }

        protected ExpressionCompileException CompileException(string messageTemplate, CompileExceptionReason reason, params object[] arguments)
        {
            var message = string.Format(messageTemplate, arguments);
            message = string.Concat(Name, ": ", message);
            return new ExpressionCompileException(message, reason);
        }

        protected ExpressionCompileException ThrowAmbiguousCallException(Type leftType, Type rightType, object operation)
        {
            return CompileException(CompileErrors.AmbiguousOverloadedOperator, CompileExceptionReason.AmbiguousMatch, leftType.Name, rightType.Name, operation);
        }

        protected YaleIlGenerator CreateTempIlGenerator(YaleIlGenerator ilgCurrent)
        {
            var dynamicMethod = new DynamicMethod("temp", typeof(int), null, GetType());
            return new YaleIlGenerator(dynamicMethod.GetILGenerator(), ilgCurrent.Length, true);
        }

        protected string Name
        {
            get
            {
                var key = GetType().Name;
                var value = ElementResourceManager.GetElementNameString(key);
                if (value == null) throw new InvalidOperationException($"Element name for '{key}' not in resource file");
                return value;
            }
        }
    }
}