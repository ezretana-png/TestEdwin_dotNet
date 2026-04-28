using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators
{
    public class AmountPerUomCalculatorTests
    {
        private readonly AmountPerUomCalculator _calculator;

        public AmountPerUomCalculatorTests()
        {
            _calculator = new AmountPerUomCalculator();
        }

        [Fact]
        public void IsValid_WhenAmountAndVolumeGreaterThanZeroAndProductSupportsIncentive_ReturnsTrue()
        {
            // Arrange
            var rebate = new Rebate { Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
            var request = new CalculateRebateRequest { Volume = 5m };

            // Act
            bool result = _calculator.IsValid(rebate, product, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenAmountIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Amount = 0m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
            var request = new CalculateRebateRequest { Volume = 5m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenVolumeIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
            var request = new CalculateRebateRequest { Volume = 0m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenProductDoesNotSupportIncentive_ReturnsFalse()
        {
            var rebate = new Rebate { Amount = 10m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
            var request = new CalculateRebateRequest { Volume = 5m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void CalculateAmount_ReturnsCorrectAmount()
        {
            var rebate = new Rebate { Amount = 7.5m };
            var product = new Product();
            var request = new CalculateRebateRequest { Volume = 10m };
            decimal expected = 7.5m * 10m; // 75

            decimal amount = _calculator.CalculateAmount(rebate, product, request);

            Assert.Equal(expected, amount);
        }
    }
}