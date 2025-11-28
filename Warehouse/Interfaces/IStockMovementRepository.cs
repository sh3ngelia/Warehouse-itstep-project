using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain;

namespace Warehouse.Interfaces
{
    public interface IStockMovementRepository : IRepository<StockMovement>
    {
        IEnumerable<StockMovement> GetByProductId(int productId);
        IEnumerable<StockMovement> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<StockMovement> GetByMovementType(StockMovementTypeEnum type);
    }
}