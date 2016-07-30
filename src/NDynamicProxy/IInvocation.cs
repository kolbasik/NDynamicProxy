using System.Reflection;

namespace kolbasik.NDynamicProxy
{
    public interface IInvocation
    {
        MethodInfo Method { get; }
        object[] Arguments { get; }
        object[] Outputs { get; }
        object ReturnValue { get; set; }
        void Proceed();
    }
}