using System;

namespace kolbasik.NDynamicProxy
{
    public sealed class DefaultValueInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = GetDefaultValue(invocation.Method.ReturnType);
        }

        private static object GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
