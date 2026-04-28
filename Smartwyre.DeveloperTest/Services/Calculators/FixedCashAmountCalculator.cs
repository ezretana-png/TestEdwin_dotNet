using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators
{
    public class FixedCashAmountCalculator : IIncentiveCalculator
    {
        public decimal CalculateAmount(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return rebate.Amount;
        }

        public bool IsValid(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                return false;
            if (rebate.Amount == 0)
                return false;
            return true;
        }
    }
}
