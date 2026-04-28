using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services.Calculators
{
    public static class IncentiveCalculatorFactory
    {
        public static IIncentiveCalculator GetCalculator(IncentiveType incentiveType) => incentiveType switch
        {
            IncentiveType.FixedCashAmount => new FixedCashAmountCalculator(),
            IncentiveType.FixedRateRebate => new FixedRateRebateCalculator(),
            IncentiveType.AmountPerUom => new AmountPerUomCalculator(),
            _ => throw new ArgumentOutOfRangeException(nameof(incentiveType), $"Unsupported incentive type: {incentiveType}")
        };
    }
}
