using Warehouse.Domain;
using Warehouse.Interfaces;

namespace Warehouse.Services
{
    public class InventoryService
    {
        private readonly IProductRepository _productRepo;
        private readonly IStockMovementRepository _movementRepo;

        public InventoryService(IProductRepository productRepo, IStockMovementRepository movementRepo)
        {
            _productRepo = productRepo;
            _movementRepo = movementRepo;
        }

        #region Product CRUD

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepo.GetAll();
        }

        public Product? GetProductById(int productId)
        {
            return _productRepo.GetById(productId);
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _productRepo.Add(product);

            if (product.Quantity > 0)
            {
                _movementRepo.Add(new StockMovement
                {
                    ProductId = product.Id,
                    QuantityChange = product.Quantity,
                    MovementType = StockMovementTypeEnum.Created,
                    Reason = "Initial stock on product creation"
                });
            }
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _productRepo.Update(product);

            _movementRepo.Add(new StockMovement
            {
                ProductId = product.Id,
                QuantityChange = 0,
                MovementType = StockMovementTypeEnum.Edited,
                Reason = "Product updated"
            });
        }

        public void DeactivateProduct(int productId)
        {
            var product = _productRepo.GetById(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found.");

            product.Deactivate();
            _productRepo.Update(product);

            _movementRepo.Add(new StockMovement
            {
                ProductId = product.Id,
                QuantityChange = 0,
                MovementType = StockMovementTypeEnum.Deactivated,
                Reason = "Product deactivated"
            });
        }

        public void DeleteProduct(int productId)
        {
            var product = _productRepo.GetById(productId);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {productId} not found.");

            product.HardDelete();
            _productRepo.HardDelete(product);

            _movementRepo.Add(new StockMovement
            {
                ProductId = productId,
                QuantityChange = 0,
                MovementType = StockMovementTypeEnum.Deleted,
                Reason = "Product permanently removed"
            });
        }

        #endregion

        #region Stock Operations

        public void IncreaseStock(int productId, int amount, string reason = "")
        {
            var product = _productRepo.GetById(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found.");

            product.IncreaseStock(amount);
            _productRepo.Update(product);

            _movementRepo.Add(new StockMovement
            {
                ProductId = productId,
                QuantityChange = amount,
                MovementType = StockMovementTypeEnum.Increased,
                Reason = string.IsNullOrWhiteSpace(reason) ? "Stock increased" : reason
            });
        }

        public void DecreaseStock(int productId, int amount, string reason = "")
        {
            var product = _productRepo.GetById(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found.");

            if (!product.DecreaseStock(amount))
                throw new InvalidOperationException("Not enough stock.");

            _productRepo.Update(product);

            _movementRepo.Add(new StockMovement
            {
                ProductId = productId,
                QuantityChange = -amount,
                MovementType = StockMovementTypeEnum.Decreased,
                Reason = string.IsNullOrWhiteSpace(reason) ? "Stock decreased" : reason
            });
        }

        #endregion

        #region History

        public IEnumerable<StockMovement> GetAllMovements()
        {
            return _movementRepo.GetAll();
        }

        public IEnumerable<StockMovement> GetMovementsByProduct(int productId)
        {
            return _movementRepo.GetByProductId(productId);
        }

        public IEnumerable<StockMovement> GetMovementsByType(StockMovementTypeEnum type)
        {
            return _movementRepo.GetByMovementType(type);
        }

        public IEnumerable<StockMovement> GetMovementsByDate(DateTime from, DateTime to)
        {
            return _movementRepo.GetByDateRange(from, to);
        }

        #endregion

        #region Summary

        public int GetTotalQuantity()
        {
            return _productRepo.GetAll().Sum(p => p.Quantity);
        }

        public decimal GetTotalInventoryValue()
        {
            return _productRepo.GetTotalInventoryValue();
        }

        public Product? GetMostExpensiveProduct()
        {
            return _productRepo.GetAll()
                .OrderByDescending(p => p.Price)
                .FirstOrDefault();
        }

        public Product? GetLeastExpensiveProduct()
        {
            return _productRepo.GetAll()
                .OrderBy(p => p.Price)
                .FirstOrDefault();
        }

        public List<Product> GetProductsByCategory(CategoryEnum category)
        {
            return _productRepo.GetByCategory(category).ToList();
        }

        public List<Product> SearchProducts(string keyword)
        {
            return _productRepo.SearchByName(keyword).ToList();
        }

        #endregion
    }
}
