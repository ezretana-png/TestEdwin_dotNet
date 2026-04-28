using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Services.Calculators
{
    public interface IIncentiveCalculator
    {
        bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request);
        decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}
