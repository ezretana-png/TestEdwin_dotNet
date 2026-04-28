using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IProductDataStore
    {
        Product GetProduct(string productIdentifier);
    }
}
