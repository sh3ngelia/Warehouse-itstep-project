
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Interfaces;

namespace Warehouse.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(CsvDataManager csv) : base(csv)
        {
        }

        protected override void LoadData()
        {
            _items.Clear();
            var products = _csv.LoadProducts();
            _items.AddRange(products);
        }

        protected override void SaveData()
        {
            _csv.SaveProducts(_items);
        }

        public IEnumerable<Product> GetByCategory(CategoryEnum category)
        {
            return _items
                .Where(p => p.Category == category && p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();
        }

        public IEnumerable<Product> GetLowStockProducts(int threshold)
        {
            return _items
                .Where(p => p.IsLowStock(threshold))
                .OrderBy(p => p.Quantity)
                .ToList();
        }

        public IEnumerable<Product> SearchByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Product>();

            string search = searchTerm.ToLower();
            return _items
                .Where(p => p.IsActive && p.Name.ToLower().Contains(search))
                .OrderBy(p => p.Name)
                .ToList();
        }

        public decimal GetTotalInventoryValue()
        {
            return _items
                .Where(p => p.IsActive)
                .Sum(p => p.TotalValue);
        }

        public Dictionary<CategoryEnum, int> GetProductCountByCategory()
        {
            return _items
                .Where(p => p.IsActive)
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public IEnumerable<Product> GetTopExpensiveProducts(int count)
        {
            return _items
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Price)
                .Take(count)
                .ToList();
        }
    }
}