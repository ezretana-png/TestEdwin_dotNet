using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services.Calculators
{
    public class AmountPerUomCalculator : IIncentiveCalculator
    {
        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount * request.Volume;
        }

        public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
                return false;
            if (rebate.Amount == 0 || request.Volume == 0)
                return false;
            return true;
        }
    }
}
