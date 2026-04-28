using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IRebateDataStore
    {
        Rebate GetRebate(string rebateIdentifier);
        bool StoreCalculationResult(Rebate account, decimal rebateAmount);
    }
}
