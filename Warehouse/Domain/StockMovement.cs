using System;

namespace Warehouse.Domain
{
    public class StockMovement : BaseEntity
    {
        private int _productId;
        private int _quantityChange;
        private string _reason = string.Empty;

        public int ProductId
        {
            get => _productId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("ProductId must be positive");
                _productId = value;
            }
        }

        public DateTime Date { get; set; }

        public int QuantityChange
        {
            get => _quantityChange;
            set => _quantityChange = value;
        }

        public StockMovementTypeEnum MovementType { get; set; }

        public string Reason
        {
            get => _reason;
            set => _reason = value?.Trim() ?? string.Empty;
        }

        public StockMovement()
        {
            Date = DateTime.Now;
        }

        public StockMovement(int productId, int quantityChange, StockMovementTypeEnum movementType, string reason)
            : this()
        {
            ProductId = productId;
            QuantityChange = quantityChange;
            MovementType = movementType;
            Reason = reason;
        }

        public override string GetEntityInfo()
        {
            string changeSign = QuantityChange >= 0 ? "+" : "";
            return $"Movement: {MovementType}, Product ID: {ProductId}, Change: {changeSign}{QuantityChange}, Date: {Date:g}, Reason: {Reason}";
        }

        public bool IsIncrease() => QuantityChange > 0;
        public bool IsDecrease() => QuantityChange < 0;
    }
}