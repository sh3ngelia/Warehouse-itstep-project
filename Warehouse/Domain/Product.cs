using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain
{
    public class Product : BaseEntity
    {
        private string _name = string.Empty;
        private decimal _price;
        private int _quantity;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");
                _name = value.Trim();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative");
                _price = value;
            }
        }

        public CategoryEnum Category { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative");
                _quantity = value;
            }
        }

        public decimal TotalValue => Price * Quantity;

        public Product() : base() { }

        public Product(string name, decimal price, CategoryEnum category, int quantity)
        {
            Name = name;
            Price = price;
            Category = category;
            Quantity = quantity;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Console.WriteLine($"Product '{Name}' (ID: {Id}) deactivated");
        }

        public override void Activate()
        {
            base.Activate();
            Console.WriteLine($"Product '{Name}' (ID: {Id}) activated");
        }

        public override void HardDelete()
        {
            base.HardDelete();
            Console.WriteLine($"Product '{Name}' (ID: {Id}) removed permanently");
        }

        public override string GetEntityInfo()
        {
            return $"Product: {Name}, Category: {Category}, Price: {Price:C}, Qty: {Quantity}, Value: {TotalValue:C}";
        }

        public bool IsLowStock(int threshold = 10)
        {
            return Quantity < threshold && IsActive;
        }

        public void IncreaseStock(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            Quantity += amount;
        }

        public bool DecreaseStock(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");

            if (Quantity >= amount)
            {
                Quantity -= amount;
                return true;
            }

            return false;
        }
    }
}
