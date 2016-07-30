using System;
using Xunit;

namespace kolbasik.NDynamicProxy.Tests
{
    public sealed class InterceptorTests
    {
        [Fact]
        public void Intercept_should_delegate_all_calls()
        {
            // arrange
            Func<string, object> interceptor = method =>
            {
                switch (method)
                {
                    case "get_ConnectionString":
                        return "AZURE";
                    case "get_Timeout":
                        return TimeSpan.FromMinutes(30);
                }
                return null;
            };

            // act
            IOptions proxy = DynamicProxy.Create<IOptions>(new SpecialNameInterceptor(interceptor));

            // assert
            Assert.Equal("AZURE", proxy.ConnectionString);
            Assert.Equal(TimeSpan.FromMinutes(30), proxy.Timeout);
            Assert.Equal(default(int?), proxy.Token);
        }

        private interface IOptions
        {
            string ConnectionString { get; }
            TimeSpan Timeout { get; }
            int? Token { get; }
        }

        private sealed class SpecialNameInterceptor : IInterceptor
        {
            private readonly Func<string, object> _interceptor;

            public SpecialNameInterceptor(Func<string, object> interceptor)
            {
                _interceptor = interceptor;
            }

            public void Intercept(IInvocation invocation)
            {
                if (invocation.Method.IsSpecialName)
                {
                    invocation.ReturnValue = _interceptor.Invoke(invocation.Method.Name);
                }
                else
                {
                    invocation.Proceed();
                }
            }
        }
    }
}