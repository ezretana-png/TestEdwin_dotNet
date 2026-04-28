using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore: IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return new Product();
        /*
         // Example of returned product
         return new Product
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };
         */
    }
}
