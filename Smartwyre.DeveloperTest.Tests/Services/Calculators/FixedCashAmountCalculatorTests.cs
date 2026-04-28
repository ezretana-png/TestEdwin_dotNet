using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators
{
    public class FixedCashAmountCalculatorTests
    {
        private readonly FixedCashAmountCalculator _calculator;

        public FixedCashAmountCalculatorTests()
        {
            _calculator = new FixedCashAmountCalculator();
        }

        [Fact]
        public void IsValid_WhenProductSupportsIncentiveAndAmountGreaterThanZero_ReturnsTrue()
        {
            // Arrange
            var rebate = new Rebate { Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
            var request = new CalculateRebateRequest();

            // Act
            bool result = _calculator.IsValid(rebate, product, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenAmountIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Amount = 0m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
            var request = new CalculateRebateRequest();

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenProductDoesNotSupportIncentive_ReturnsFalse()
        {
            var rebate = new Rebate { Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
            var request = new CalculateRebateRequest();

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void CalculateAmount_ReturnsCorrectAmount()
        {
            var rebate = new Rebate { Amount = 75.5m };
            var product = new Product();
            var request = new CalculateRebateRequest();

            decimal amount = _calculator.CalculateAmount(rebate, product, request);

            Assert.Equal(75.5m, amount);
        }
    }
}
