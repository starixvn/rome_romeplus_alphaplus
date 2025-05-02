using AlphaPlus.CashService.Site.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments.Models
{
    public class PaymentMethodModelTests
    {
        [Fact]
        public void CreateOptionAll_WhenCalled_ReturnsAllOption()
        {
            // Act
            var result = PaymentMethodModel.CreateOptionAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("All", result.Name);
        }
    }
} 