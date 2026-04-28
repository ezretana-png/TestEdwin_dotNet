using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services.Calculators
{
    public class FixedRateRebateCalculator : IIncentiveCalculator
    {
        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return product.Price * rebate.Percentage * request.Volume;
        }

        public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                return false;
            if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
                return false;
            return true;
        }
    }
}
