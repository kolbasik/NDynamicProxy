namespace kolbasik.NDynamicProxy
{
    public interface IInterceptor
    {
        void Intercept(IInvocation invocation);
    }
}