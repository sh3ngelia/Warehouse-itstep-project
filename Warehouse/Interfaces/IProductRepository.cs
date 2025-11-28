using System.Collections.Generic;
using Warehouse.Domain;

namespace Warehouse.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetByCategory(CategoryEnum category);
        IEnumerable<Product> GetLowStockProducts(int threshold);
        IEnumerable<Product> SearchByName(string searchTerm);
        decimal GetTotalInventoryValue();
    }
}