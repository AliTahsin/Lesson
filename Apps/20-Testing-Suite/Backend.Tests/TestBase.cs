using Xunit;

namespace Backend.Tests
{
    public class TestBase : IDisposable
    {
        protected readonly Mock<IServiceProvider> _serviceProvider;
        protected readonly Mock<ILogger<TestBase>> _logger;

        public TestBase()
        {
            _serviceProvider = new Mock<IServiceProvider>();
            _logger = new Mock<ILogger<TestBase>>();
        }

        public void Dispose()
        {
            // Cleanup
        }

        protected T CreateMock<T>() where T : class
        {
            return new Mock<T>().Object;
        }
    }
}