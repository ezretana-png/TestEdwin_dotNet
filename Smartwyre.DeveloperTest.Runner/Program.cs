using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        // Parse command line arguments: --rebate R123 --product P456 --volume 10
        string rebateId = GetArgument(args, "--rebate");
        string productId = GetArgument(args, "--product");
        decimal volume = decimal.TryParse(GetArgument(args, "--volume"), out var vol) ? vol : 0m;

        if (string.IsNullOrWhiteSpace(rebateId) || string.IsNullOrWhiteSpace(productId))
        {
            Console.WriteLine("Usage: dotnet run -- --rebate <rebateId> --product <productId> [--volume <volume>]");
            return;
        }

        // Instantiate concrete stores (no DI container needed for this simple runner)
        IRebateDataStore rebateStore = new RebateDataStore();
        IProductDataStore productStore = new ProductDataStore();
        var service = new RebateService(rebateStore, productStore);

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateId,
            ProductIdentifier = productId,
            Volume = volume
        };

        CalculateRebateResult result = service.Calculate(request);

        Console.WriteLine($"Success: {result.Success}");
    }

    private static string GetArgument(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
    }
}
