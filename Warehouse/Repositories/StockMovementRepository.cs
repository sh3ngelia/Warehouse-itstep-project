using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Interfaces;

namespace Warehouse.Repositories
{
    public class StockMovementRepository : BaseRepository<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(CsvDataManager csv) : base(csv)
        {
        }

        protected override void LoadData()
        {
            _items.Clear();
            var movements = _csv.LoadMovements();
            _items.AddRange(movements);
        }

        protected override void SaveData()
        {
            _csv.SaveMovements(_items);
        }

        public IEnumerable<StockMovement> GetByProductId(int productId)
        {
            return _items
                .Where(m => m.ProductId == productId && m.IsActive)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        public IEnumerable<StockMovement> GetByDateRange(DateTime start, DateTime end)
        {
            return _items
                .Where(m => m.IsActive && m.Date >= start && m.Date <= end)
                .OrderBy(m => m.Date)
                .ToList();
        }

        public IEnumerable<StockMovement> GetByMovementType(StockMovementTypeEnum type)
        {
            return _items
                .Where(m => m.MovementType == type && m.IsActive)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        public int GetTotalQuantityChangeForProduct(int productId)
        {
            return _items
                .Where(m => m.ProductId == productId && m.IsActive)
                .Sum(m => m.QuantityChange);
        }

        public Dictionary<StockMovementTypeEnum, int> GetMovementCountByType()
        {
            return _items
                .Where(m => m.IsActive)
                .GroupBy(m => m.MovementType)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}