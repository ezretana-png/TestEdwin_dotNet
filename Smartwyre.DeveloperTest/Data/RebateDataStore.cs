using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore:IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return new Rebate();
        /*
         // Example of returned rebate
         return new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 100m
        };
         */
    }

    public bool StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity

        return true;
    }
}
