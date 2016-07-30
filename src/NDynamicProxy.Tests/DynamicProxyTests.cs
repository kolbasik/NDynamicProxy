using System;
using Xunit;

namespace kolbasik.NDynamicProxy.Tests
{
    public sealed class DynamicProxyTests
    {
        [Fact]
        public void Ctor_should_create_a_real_proxy()
        {
            // act
            IOptions proxy = new DynamicProxy<IOptions>().Object;

            // assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Create_should_create_a_real_proxy()
        {
            // act
            var proxy = DynamicProxy.Create<IOptions>(new DefaultValueInterceptor());

            // assert
            Assert.NotNull(proxy);
            Assert.Equal(default(TimeSpan), proxy.Timeout);
        }

        [Fact]
        public void Invoke_should_raise_an_exception_if_interceptor_is_undefined()
        {
            // act
            var proxy = DynamicProxy.Create<IOptions>();

            // assert
            Assert.ThrowsAny<NotSupportedException>(() => proxy.Timeout);
        }

        [Fact]
        public void Invoke_should_return_values_if_interceptor_is_defined()
        {
            // act
            var proxy = DynamicProxy.Create<IOptions>(new DefaultValueInterceptor());

            // assert
            Assert.Equal(default(string), proxy.ConnectionString);
            Assert.Equal(default(TimeSpan), proxy.Timeout);
            Assert.Equal(default(int?), proxy.Token);
        }

        private interface IOptions
        {
            string ConnectionString { get; }
            TimeSpan Timeout { get; }
            int? Token { get; }
        }
    }
}
