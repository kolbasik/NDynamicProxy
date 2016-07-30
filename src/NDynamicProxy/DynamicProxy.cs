namespace kolbasik.NDynamicProxy
{
    public static class DynamicProxy
    {
        public static T Create<T>(params IInterceptor[] interceptors)
        {
            return new DynamicProxy<T>(interceptors).Object;
        }
    }
}