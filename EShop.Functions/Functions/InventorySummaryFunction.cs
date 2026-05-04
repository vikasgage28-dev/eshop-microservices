using EShop.Core.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EShop.Functions.Functions
{
    public class InventorySummaryFunction
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<InventorySummaryFunction> _logger;

        public InventorySummaryFunction(
            IProductRepository productRepository,
            ILogger<InventorySummaryFunction> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [Function("InventorySummary")]
        public async Task Run(
            [TimerTrigger("0 0 0 * * *")] TimerInfo timer)
        {
            _logger.LogInformation("Inventory Summary started at: {time}", DateTime.UtcNow);

            var products = await _productRepository.GetAllAsync();
            var productList = products.ToList();

            // Summary per category
            var categorySummary = productList
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key, Count = g.Count(), TotalStock = g.Sum(p => p.Stock) });

            foreach (var category in categorySummary)
            {
                _logger.LogInformation(
                    "Category: {category} | Products: {count} | Total Stock: {stock}",
                    category.Category, category.Count, category.TotalStock);
            }

            // Low stock warnings
            var lowStock = productList.Where(p => p.Stock < 5).ToList();

            if (lowStock.Any())
            {
                _logger.LogWarning("LOW STOCK ALERT! {count} products need restocking!", lowStock.Count);
                foreach (var product in lowStock)
                {
                    _logger.LogWarning(
                        "⚠️ LOW STOCK: {name} | Stock: {stock}",
                        product.Name, product.Stock);
                }
            }
            else
            {
                _logger.LogInformation("All products have sufficient stock ✅");
            }

            _logger.LogInformation("Inventory Summary completed at: {time}", DateTime.UtcNow);
        }
    }
}