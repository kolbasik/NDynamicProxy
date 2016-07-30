using System;
using System.Collections.Generic;
using System.Reflection;

namespace kolbasik.NDynamicProxy
{
    public sealed class Pipeline
    {
        private readonly List<IInterceptor> _interceptors;

        public Pipeline(params IInterceptor[] interceptors)
        {
            if (interceptors == null) throw new ArgumentNullException(nameof(interceptors));
            _interceptors = new List<IInterceptor>(interceptors);
        }

        public object Process(MethodInfo method, object[] arguments, out object[] outputs)
        {
            Invocation invocation = null;
            var enumerator = _interceptors.GetEnumerator();
            invocation = new Invocation(method, arguments, () =>
            {
                if (enumerator.MoveNext())
                {
                    enumerator.Current?.Intercept(invocation);
                }
                else
                {
                    throw new NotSupportedException(method.Name);
                }
            });
            invocation.Proceed();
            outputs = invocation.Outputs ?? new object[0];
            return invocation.ReturnValue;
        }

        private sealed class Invocation : IInvocation
        {
            private readonly Action _proceed;

            public Invocation(MethodInfo method, object[] arguments, Action proceed)
            {
                Method = method;
                Arguments = arguments;
                _proceed = proceed;
            }

            public MethodInfo Method { get; }
            public object[] Arguments { get; }
            public object[] Outputs { get; set; }
            public object ReturnValue { get; set; }

            public void Proceed()
            {
                _proceed.Invoke();
            }
        }
    }
}