using System;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    [Fact]
    public void Calculate_WhenRebateIsNull_ReturnsSuccessFalse()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        // set up the GetRebate return null
        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns((Rebate)null);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(new Product());

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WhenProductIsNull_ReturnsSuccessFalse()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        // Rebate is valid, producto is null
        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(new Rebate());
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns((Product)null);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithValidFixedCashAmount_ReturnsSuccessTrue()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);
        rebateStoreMock.Setup(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>())).Returns(true);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        // Assert
        Assert.True(result.Success);
        rebateStoreMock.Verify(r => r.StoreCalculationResult(rebate, 100m), Times.Once);
    }

    [Fact]
    public void Calculate_WithFixedCashAmount_WhenAmountIsZero_ReturnsSuccessFalse()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 0m          // <-- invalid amount for FixedCashAmount
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);        

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        // Assert
        Assert.False(result.Success);
        rebateStoreMock.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }
        
    [Fact]
    public void Calculate_WithFixedCashAmount_WhenProductDoesNotSupportIncentive_ReturnsSuccessFalse()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        };

        var product = new Product
        {
            // Does Not Support Incentive FixedCashAmount
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        // Assert
        Assert.False(result.Success);
        rebateStoreMock.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_WithValidFixedRateRebate_ReturnsSuccessTrue()
    {
        // Arrange
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.05m
        };

        var product = new Product
        {
            Price = 200m,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1",
            Volume = 10m
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);
        rebateStoreMock.Setup(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>())).Returns(true);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act
        var result = service.Calculate(request);

        // Assert
        Assert.True(result.Success);
        decimal expectedAmount = product.Price * rebate.Percentage * request.Volume; // 200 * 0.05 * 10 = 100
        rebateStoreMock.Verify(r => r.StoreCalculationResult(rebate, expectedAmount), Times.Once);
    }

    [Fact]
    public void Calculate_WithFixedRateRebate_WhenPercentageIsZero_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0m };
        var product = new Product { Price = 100m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithFixedRateRebate_WhenPriceIsZero_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
        var product = new Product { Price = 0m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 10m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithFixedRateRebate_WhenVolumeIsZero_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
        var product = new Product { Price = 100m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
        var request = new CalculateRebateRequest { Volume = 0m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithFixedRateRebate_WhenProductDoesNotSupportIncentive_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
        var product = new Product { Price = 100m, SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 10m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithValidAmountPerUom_ReturnsSuccessTrue()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.AmountPerUom,
            Amount = 5m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1",
            Volume = 30m
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);
        rebateStoreMock.Setup(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>())).Returns(true);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.True(result.Success);
        decimal expectedAmount = rebate.Amount * request.Volume; // 5 * 30 = 150
        rebateStoreMock.Verify(r => r.StoreCalculationResult(rebate, expectedAmount), Times.Once);
    }

    [Fact]
    public void Calculate_WithAmountPerUom_WhenAmountIsZero_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 0m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 10m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithAmountPerUom_WhenVolumeIsZero_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 5m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        var request = new CalculateRebateRequest { Volume = 0m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WithAmountPerUom_WhenProductDoesNotSupportIncentive_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 5m };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount };
        var request = new CalculateRebateRequest { Volume = 10m };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_WhenStoreCalculationResultFails_ReturnsSuccessFalse()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 50m
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        rebateStoreMock.Setup(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>())).Returns(false);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);
        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "reb1",
            ProductIdentifier = "prod1"
        });

        Assert.False(result.Success);

        rebateStoreMock.Verify(r => r.StoreCalculationResult(rebate, 50m), Times.Once);
    }

    [Fact]
    public void Calculate_WithUnsupportedIncentiveType_ThrowsArgumentOutOfRangeException()
    {
        var rebateStoreMock = new Mock<IRebateDataStore>();
        var productStoreMock = new Mock<IProductDataStore>();

        // Unsupported IncentiveType
        var rebate = new Rebate
        {
            Incentive = (IncentiveType)999
        };

        var product = new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        rebateStoreMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productStoreMock.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        var service = new RebateService(rebateStoreMock.Object, productStoreMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.Calculate(new CalculateRebateRequest
            {
                RebateIdentifier = "reb1",
                ProductIdentifier = "prod1"
            }));
    }
}