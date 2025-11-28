using Warehouse.Domain;
using Warehouse.Services;

namespace Warehouse.UI
{
    public class Menu
    {
        private readonly InventoryService _service;

        public Menu(InventoryService service)
        {
            _service = service;
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("WAREHOUSE MANAGEMENT SYSTEM");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("  📦 PRODUCT MANAGEMENT");
                Console.ResetColor();
                Console.WriteLine("    1. List All Products");
                Console.WriteLine("    2. Create New Product");
                Console.WriteLine("    3. Update Product");
                Console.WriteLine("    4. Deactivate Product");
                Console.WriteLine("    5. Delete Product (Permanent)");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  📊 STOCK OPERATIONS");
                Console.ResetColor();
                Console.WriteLine("    6. Increase Stock");
                Console.WriteLine("    7. Decrease Stock");
                Console.WriteLine("    8. View Stock Movements");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  🔍 SEARCH & REPORTS");
                Console.ResetColor();
                Console.WriteLine("    9. Search Products");
                Console.WriteLine("    10. Filter By Category");
                Console.WriteLine("    11. Inventory Summary");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    0. Exit System");
                Console.ResetColor();

                Console.WriteLine();
                PrintSeparator();
                Console.Write("Choose option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1": ShowProducts(); break;
                    case "2": CreateProduct(); break;
                    case "3": UpdateProduct(); break;
                    case "4": DeactivateProduct(); break;
                    case "5": DeleteProduct(); break;
                    case "6": IncreaseStock(); break;
                    case "7": DecreaseStock(); break;
                    case "8": ShowMovements(); break;
                    case "9": SearchProduct(); break;
                    case "10": FilterByCategory(); break;
                    case "11": Summary(); break;
                    case "0":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n  ✓ Thank you for using Warehouse Management System!\n");
                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n  ✗ Invalid choice! Please try again.");
                        Console.ResetColor();
                        Pause();
                        break;
                }
            }
        }

        #region ProductOperations
        private void ShowProducts()
        {
            Console.Clear();
            PrintHeader("PRODUCT LIST");

            var products = _service.GetAllProducts().ToList();

            if (!products.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  ⚠ No products found in inventory.\n");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine();
            PrintTableHeader(new[] { "ID", "Product Name", "Category", "Price", "Quantity", "Total Value", "Status" });

            foreach (var p in products)
            {
                string status = p.IsActive ? "✓ Active" : "✗ Inactive";
                ConsoleColor statusColor = p.IsActive ? ConsoleColor.Green : ConsoleColor.Red;

                Console.Write($"  {p.Id,-4}");
                Console.Write($" {TruncateString(p.Name, 25),-25}");
                Console.Write($" {p.Category,-12}");
                Console.Write($" {p.Price,10:C}");

                if (p.Quantity == 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (p.Quantity < 10)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($" {p.Quantity,8}");
                Console.ResetColor();

                Console.Write($" {p.TotalValue,12:C}");

                Console.ForegroundColor = statusColor;
                Console.WriteLine($" {status,-10}");
                Console.ResetColor();
            }

            PrintSeparator();
            Console.WriteLine($"  Total Products: {products.Count}");

            Pause();
        }

        private void CreateProduct()
        {
            Console.Clear();
            PrintHeader("CREATE NEW PRODUCT");
            Console.WriteLine();

            Console.Write("  Product Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("  Price: ₾");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("  Initial Quantity: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Available Categories:");
            Console.ResetColor();
            foreach (var c in Enum.GetValues(typeof(CategoryEnum)))
                Console.WriteLine($"    {(int)c}. {c}");

            Console.Write("\n  Choose category number: ");
            int cat = int.Parse(Console.ReadLine() ?? "1");

            var product = new Product(name, price, (CategoryEnum)cat, qty);
            _service.CreateProduct(product);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✓ Product '{name}' created successfully with ID: {product.Id}");
            Console.ResetColor();
            Pause();
        }

        private void UpdateProduct()
        {
            Console.Clear();
            PrintHeader("UPDATE PRODUCT");
            Console.WriteLine();

            int id = AskInt("  Enter Product ID: ");
            var product = _service.GetProductById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  ✗ Product not found.");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Current Product Details:");
            Console.ResetColor();
            Console.WriteLine($"    Name: {product.Name}");
            Console.WriteLine($"    Price: {product.Price:C}");
            Console.WriteLine($"    Quantity: {product.Quantity}");
            Console.WriteLine($"    Category: {product.Category}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  (Press Enter to keep current value)");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write($"  New Name [{product.Name}]: ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                product.Name = name;

            Console.Write($"  New Price [{product.Price:C}]: $");
            string? priceInput = Console.ReadLine();
            if (decimal.TryParse(priceInput, out decimal newPrice))
                product.Price = newPrice;

            Console.Write($"  New Quantity [{product.Quantity}]: ");
            string? qtyInput = Console.ReadLine();
            if (int.TryParse(qtyInput, out int newQty))
                product.Quantity = newQty;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Available Categories:");
            Console.ResetColor();
            foreach (var c in Enum.GetValues(typeof(CategoryEnum)))
                Console.WriteLine($"    {(int)c}. {c}");

            Console.Write($"\n  New Category [{product.Category}]: ");
            string? catInput = Console.ReadLine();
            if (int.TryParse(catInput, out int newCat))
                product.Category = (CategoryEnum)newCat;

            _service.UpdateProduct(product);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Product updated successfully.");
            Console.ResetColor();
            Pause();
        }

        private void DeactivateProduct()
        {
            Console.Clear();
            PrintHeader("DEACTIVATE PRODUCT");
            Console.WriteLine();

            int id = AskInt("  Enter Product ID: ");
            var product = _service.GetProductById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  ✗ Product not found.");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"  Product: {product.Name}");
            Console.Write("  Are you sure you want to deactivate? (Y/N): ");
            string confirm = Console.ReadLine()?.ToUpper() ?? "N";

            if (confirm == "Y")
            {
                _service.DeactivateProduct(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  ✓ Product deactivated successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  ⚠ Operation cancelled.");
                Console.ResetColor();
            }

            Pause();
        }

        private void DeleteProduct()
        {
            Console.Clear();
            PrintHeader("DELETE PRODUCT (PERMANENT)");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ⚠ WARNING: This action cannot be undone!");
            Console.ResetColor();
            Console.WriteLine();

            int id = AskInt("  Enter Product ID: ");
            var product = _service.GetProductById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  ✗ Product not found.");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine($"  Product: {product.Name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  Type 'DELETE' to confirm: ");
            Console.ResetColor();
            string confirm = Console.ReadLine() ?? "";

            if (confirm == "DELETE")
            {
                _service.DeleteProduct(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  ✓ Product permanently deleted.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  ⚠ Operation cancelled.");
                Console.ResetColor();
            }

            Pause();
        }
        #endregion

        #region StockOperations
        private void IncreaseStock()
        {
            Console.Clear();
            PrintHeader("INCREASE STOCK");
            Console.WriteLine();

            int id = AskInt("  Product ID: ");
            var product = _service.GetProductById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  ✗ Product not found.");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine($"\n  Product: {product.Name}");
            Console.WriteLine($"  Current Stock: {product.Quantity}");
            Console.WriteLine();

            int amount = AskInt("  Amount to Add: ");

            Console.Write("  Reason: ");
            string? reason = Console.ReadLine();

            _service.IncreaseStock(id, amount, reason ?? "");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✓ Stock increased. New quantity: {product.Quantity + amount}");
            Console.ResetColor();
            Pause();
        }

        private void DecreaseStock()
        {
            Console.Clear();
            PrintHeader("DECREASE STOCK");
            Console.WriteLine();

            int id = AskInt("  Product ID: ");
            var product = _service.GetProductById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  ✗ Product not found.");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine($"\n  Product: {product.Name}");
            Console.WriteLine($"  Current Stock: {product.Quantity}");
            Console.WriteLine();

            int amount = AskInt("  Amount to Remove: ");

            Console.Write("  Reason: ");
            string? reason = Console.ReadLine();

            try
            {
                _service.DecreaseStock(id, amount, reason ?? "");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  ✓ Stock decreased. New quantity: {product.Quantity - amount}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ✗ Error: {ex.Message}");
                Console.ResetColor();
            }

            Pause();
        }
        #endregion

        #region History
        private void ShowMovements()
        {
            Console.Clear();
            PrintHeader("STOCK MOVEMENT HISTORY");

            var movements = _service.GetAllMovements().ToList();

            if (!movements.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  ⚠ No stock movements found.\n");
                Console.ResetColor();
                Pause();
                return;
            }

            Console.WriteLine();
            PrintTableHeader(new[] { "ID", "Product ID", "Type", "Change", "Date", "Reason" });

            foreach (var m in movements)
            {
                string changeStr = m.QuantityChange >= 0 ? $"+{m.QuantityChange}" : m.QuantityChange.ToString();
                ConsoleColor changeColor = m.QuantityChange > 0 ? ConsoleColor.Green :
                                          m.QuantityChange < 0 ? ConsoleColor.Red :
                                          ConsoleColor.Gray;

                Console.Write($"  {m.Id,-4}");
                Console.Write($" {m.ProductId,-11}");
                Console.Write($" {m.MovementType,-12}");

                Console.ForegroundColor = changeColor;
                Console.Write($" {changeStr,7}");
                Console.ResetColor();

                Console.Write($" {m.Date,-20:yyyy-MM-dd HH:mm}");
                Console.WriteLine($" {TruncateString(m.Reason, 30)}");
            }

            PrintSeparator();
            Console.WriteLine($"  Total Movements: {movements.Count}");

            Pause();
        }
        #endregion

        #region Search
        private void SearchProduct()
        {
            Console.Clear();
            PrintHeader("SEARCH PRODUCTS");
            Console.WriteLine();

            Console.Write("  Enter search keyword: ");
            string keyword = Console.ReadLine() ?? "";

            var results = _service.SearchProducts(keyword).ToList();

            Console.WriteLine();
            if (!results.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  ⚠ No products found matching your search.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Found {results.Count} product(s):");
                Console.ResetColor();
                Console.WriteLine();

                PrintTableHeader(new[] { "ID", "Product Name", "Category", "Price", "Quantity" });

                foreach (var p in results)
                {
                    Console.Write($"  {p.Id,-4}");
                    Console.Write($" {TruncateString(p.Name, 25),-25}");
                    Console.Write($" {p.Category,-12}");
                    Console.Write($" {p.Price,10:C}");
                    Console.WriteLine($" {p.Quantity,8}");
                }
            }

            Pause();
        }

        private void FilterByCategory()
        {
            Console.Clear();
            PrintHeader("FILTER BY CATEGORY");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Available Categories:");
            Console.ResetColor();
            foreach (var c in Enum.GetValues(typeof(CategoryEnum)))
                Console.WriteLine($"    {(int)c}. {c}");

            Console.WriteLine();
            int cat = AskInt("  Choose category: ");

            var results = _service.GetProductsByCategory((CategoryEnum)cat).ToList();

            Console.WriteLine();
            if (!results.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  ⚠ No products found in category: {(CategoryEnum)cat}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Products in {(CategoryEnum)cat} ({results.Count}):");
                Console.ResetColor();
                Console.WriteLine();

                PrintTableHeader(new[] { "ID", "Product Name", "Price", "Quantity", "Total Value" });

                foreach (var p in results)
                {
                    Console.Write($"  {p.Id,-4}");
                    Console.Write($" {TruncateString(p.Name, 30),-30}");
                    Console.Write($" {p.Price,10:C}");
                    Console.Write($" {p.Quantity,8}");
                    Console.WriteLine($" {p.TotalValue,12:C}");
                }
            }

            Pause();
        }

        private void Summary()
        {
            Console.Clear();
            PrintHeader("INVENTORY SUMMARY REPORT");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  📊 Overall Statistics");
            Console.ResetColor();
            Console.WriteLine($"    Total Products: {_service.GetAllProducts().Count()}");
            Console.WriteLine($"    Total Items in Stock: {_service.GetTotalQuantity():N0}");
            Console.WriteLine($"    Total Inventory Value: {_service.GetTotalInventoryValue():C}");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  💰 Price Analysis");
            Console.ResetColor();

            var expensive = _service.GetMostExpensiveProduct();
            if (expensive != null)
                Console.WriteLine($"    Most Expensive: {expensive.Name} ({expensive.Price:C})");

            var cheap = _service.GetLeastExpensiveProduct();
            if (cheap != null)
                Console.WriteLine($"    Least Expensive: {cheap.Name} ({cheap.Price:C})");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  📈 Category Breakdown");
            Console.ResetColor();

            foreach (var cat in Enum.GetValues(typeof(CategoryEnum)))
            {
                var count = _service.GetProductsByCategory((CategoryEnum)cat).Count();
                if (count > 0)
                    Console.WriteLine($"    {cat}: {count} product(s)");
            }

            Pause();
        }
        #endregion

        #region Helpers
        //helpers
        private int AskInt(string message)
        {
            Console.Write(message);
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ✗ Invalid number, try again: ");
                Console.ResetColor();
            }
            return num;
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private void PrintHeader(string title)
        {
            int width = 70;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + new string('═', width));
            Console.WriteLine($"  ║{title.PadLeft((width + title.Length) / 2).PadRight(width - 2)}║");
            Console.WriteLine("  " + new string('═', width));
            Console.ResetColor();
        }

        private void PrintSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  " + new string('-', 70));
            Console.ResetColor();
        }

        private void PrintTableHeader(string[] columns)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ");
            foreach (var col in columns)
            {
                if (col == "ID")
                    Console.Write($"{col,-4} ");
                else if (col == "Product Name")
                    Console.Write($"{col,-25} ");
                else if (col == "Category")
                    Console.Write($"{col,-12} ");
                else if (col == "Price" || col == "Total Value")
                    Console.Write($"{col,10} ");
                else if (col == "Quantity" || col == "Change")
                    Console.Write($"{col,8} ");
                else if (col == "Status")
                    Console.Write($"{col,-10} ");
                else if (col == "Product ID")
                    Console.Write($"{col,-11} ");
                else if (col == "Type")
                    Console.Write($"{col,-12} ");
                else if (col == "Date")
                    Console.Write($"{col,-20} ");
                else if (col == "Reason")
                    Console.Write($"{col,-30}");
                else
                    Console.Write($"{col,-15} ");
            }
            Console.WriteLine();
            PrintSeparator();
            Console.ResetColor();
        }

        private string TruncateString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
                return str;
            return str.Substring(0, maxLength - 3) + "...";
        }
        #endregion
    }
}
