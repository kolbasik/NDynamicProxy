using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace kolbasik.NDynamicProxy
{
    public sealed class DynamicProxy<T> : RealProxy
    {
        private readonly Pipeline _pipeline;

        public DynamicProxy(params IInterceptor[] interceptors) : base(typeof (T))
        {
            _pipeline = new Pipeline(interceptors);
        }

        public T Object => (T) GetTransparentProxy();

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage) msg;
            try
            {
                object[] outputs;
                var result = _pipeline.Process((MethodInfo) methodCall.MethodBase, methodCall.Args, out outputs);
                return new ReturnMessage(result, outputs, outputs.Length, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, methodCall);
            }
        }
    }
}