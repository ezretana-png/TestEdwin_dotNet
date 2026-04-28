using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators
{
    public class FixedRateRebateCalculatorTests
    {
        private readonly FixedRateRebateCalculator _calculator;

        public FixedRateRebateCalculatorTests()
        {
            _calculator = new FixedRateRebateCalculator();
        }

        [Fact]
        public void IsValid_WhenAllValuesGreaterThanZeroAndProductSupportsIncentive_ReturnsTrue()
        {
            // Arrange
            var rebate = new Rebate { Percentage = 0.1m };
            var product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            };
            var request = new CalculateRebateRequest { Volume = 5m };

            // Act
            bool result = _calculator.IsValid(rebate, product, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenPercentageIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Percentage = 0m };
            var product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            };
            var request = new CalculateRebateRequest { Volume = 5m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenPriceIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Percentage = 0.1m };
            var product = new Product
            {
                Price = 0m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            };
            var request = new CalculateRebateRequest { Volume = 5m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenVolumeIsZero_ReturnsFalse()
        {
            var rebate = new Rebate { Percentage = 0.1m };
            var product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            };
            var request = new CalculateRebateRequest { Volume = 0m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenProductDoesNotSupportIncentive_ReturnsFalse()
        {
            var rebate = new Rebate { Percentage = 0.1m };
            var product = new Product
            {
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };
            var request = new CalculateRebateRequest { Volume = 5m };

            bool result = _calculator.IsValid(rebate, product, request);

            Assert.False(result);
        }

        [Fact]
        public void CalculateAmount_ReturnsCorrectAmount()
        {
            var rebate = new Rebate { Percentage = 0.25m };
            var product = new Product { Price = 200m };
            var request = new CalculateRebateRequest { Volume = 4m };
            decimal expected = 200m * 0.25m * 4m; // 200

            decimal amount = _calculator.CalculateAmount(rebate, product, request);

            Assert.Equal(expected, amount);
        }
    }
}