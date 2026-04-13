using Xunit;
using Moq;
using FluentAssertions;
using PaymentInvoice.API.Services;
using PaymentInvoice.API.DTOs;

namespace Backend.Tests.UnitTests
{
    public class PaymentTests : TestBase
    {
        private readonly PaymentService _service;
        private readonly Mock<IPaymentRepository> _mockRepository;

        public PaymentTests()
        {
            _mockRepository = new Mock<IPaymentRepository>();
            _service = new PaymentService();
        }

        [Fact]
        public void GetPaymentById_WithValidId_ShouldReturnPayment()
        {
            // Arrange
            var validId = 1;

            // Act
            var result = _service.GetPaymentByIdAsync(validId).Result;

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(validId);
        }

        [Fact]
        public void ProcessPayment_WithValidCard_ShouldSucceed()
        {
            // Arrange
            var request = new PaymentRequestDto
            {
                Amount = 100,
                Currency = "EUR",
                CardNumber = "4111111111111111",
                CardHolderName = "TEST USER",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                Cvv = "123",
                PaymentMethod = "CreditCard"
            };

            // Act
            var result = _service.ProcessPaymentAsync(request).Result;

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ProcessPayment_WithInvalidAmount_ShouldThrowException(decimal amount)
        {
            // Arrange
            var request = new PaymentRequestDto
            {
                Amount = amount,
                Currency = "EUR"
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.ProcessPaymentAsync(request));
        }
    }
}