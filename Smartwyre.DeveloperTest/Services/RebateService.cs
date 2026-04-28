using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService: IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        //validate rebate and product exist
        if (rebate == null || product == null)
        {
            result.Success = false;
            return result;
        }

        var calculator = IncentiveCalculatorFactory.GetCalculator(rebate.Incentive);

        //bussines rules for this incentive
        if (!calculator.IsValid(rebate, product, request))
        {
            result.Success = false;
            return result;
        }

        //calculate rebate amount
        var rebateAmount = calculator.CalculateAmount(rebate, product, request);
        //store result and return the success of the operation        
        result.Success = _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return result;
    }
}
