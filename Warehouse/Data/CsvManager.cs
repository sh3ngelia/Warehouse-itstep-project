using System.Text;
using Warehouse.Domain;

namespace Warehouse.Data
{
    public class CsvDataManager
    {
        private readonly string _productsFilePath;
        private readonly string _movementsFilePath;
        private readonly Encoding _encoding;

        public CsvDataManager(string dataDirectory = "Data")
        {
            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            _productsFilePath = Path.Combine(dataDirectory, "products.csv");
            _movementsFilePath = Path.Combine(dataDirectory, "movements.csv");
            _encoding = Encoding.UTF8;

            InitializeFiles();
        }

        private void InitializeFiles()
        {
            if (!File.Exists(_productsFilePath))
                CreateProductsFile();

            if (!File.Exists(_movementsFilePath))
                CreateMovementsFile();
        }

        private void CreateProductsFile()
        {
            using (FileStream fs = new FileStream(_productsFilePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fs, _encoding))
            {
                writer.WriteLine("Id,Name,Price,Category,Quantity,IsActive,CreatedAt");
            }
            Console.WriteLine($"[INFO] Created products file: {_productsFilePath}");
        }

        private void CreateMovementsFile()
        {
            using (FileStream fs = new FileStream(_movementsFilePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fs, _encoding))
            {
                writer.WriteLine("Id,ProductId,QuantityChange,MovementType,Reason,Date,IsActive,CreatedAt");
            }
            Console.WriteLine($"[INFO] Created movements file: {_movementsFilePath}");
        }

        public List<Product> LoadProducts()
        {
            var products = new List<Product>();

            try
            {
                using (FileStream fs = new FileStream(_productsFilePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs, _encoding))
                {
                    reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()!) != null)
                    {
                        try
                        {
                            var product = ParseProductLine(line);
                            if (product != null)
                                products.Add(product);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] Failed to parse product line: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"[INFO] Loaded {products.Count} products from file");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[INFO] Products file not found. Starting with empty inventory.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load products: {ex.Message}");
            }

            return products;
        }

        public List<StockMovement> LoadMovements()
        {
            var movements = new List<StockMovement>();

            try
            {
                using (FileStream fs = new FileStream(_movementsFilePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs, _encoding))
                {
                    reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()!) != null)
                    {
                        try
                        {
                            var movement = ParseMovementLine(line);
                            if (movement != null)
                                movements.Add(movement);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] Failed to parse movement line: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"[INFO] Loaded {movements.Count} movements from file");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[INFO] Movements file not found. Starting with empty history.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load movements: {ex.Message}");
            }

            return movements;
        }
        public void SaveProducts(IEnumerable<Product> products)
        {
            try
            {
                using (FileStream fs = new FileStream(_productsFilePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs, _encoding))
                {
                    writer.WriteLine("Id,Name,Price,Category,Quantity,IsActive,CreatedAt");

                    foreach (var product in products)
                    {
                        writer.WriteLine(FormatProductLine(product));
                    }
                }

                Console.WriteLine($"[INFO] Saved {products.Count()} products to file");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to save products: {ex.Message}");
            }
        }

        public void SaveMovements(IEnumerable<StockMovement> movements)
        {
            try
            {
                using (FileStream fs = new FileStream(_movementsFilePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs, _encoding))
                {
                    writer.WriteLine("Id,ProductId,QuantityChange,MovementType,Reason,Date,IsActive,CreatedAt");

                    foreach (var movement in movements)
                    {
                        writer.WriteLine(FormatMovementLine(movement));
                    }
                }

                Console.WriteLine($"[INFO] Saved {movements.Count()} movements to file");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to save movements: {ex.Message}");
            }
        }

        #region Helpers
        private Product ParseProductLine(string line)
        {
            var parts = line.Split(',');
            if (parts.Length < 7)
                return null!;

            var product = new Product
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Category = (CategoryEnum)int.Parse(parts[3]),
                Quantity = int.Parse(parts[4])
            };

            typeof(BaseEntity).GetProperty("IsActive")?.SetValue(product, bool.Parse(parts[5]));
            typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(product, DateTime.Parse(parts[6]));

            return product;
        }

        private StockMovement ParseMovementLine(string line)
        {
            var parts = SplitCsvLine(line);
            if (parts.Length < 8)
                return null!;

            var movement = new StockMovement
            {
                Id = int.Parse(parts[0]),
                ProductId = int.Parse(parts[1]),
                QuantityChange = int.Parse(parts[2]),
                MovementType = (StockMovementTypeEnum)int.Parse(parts[3]),
                Reason = parts[4],
                Date = DateTime.Parse(parts[5])
            };

            typeof(BaseEntity).GetProperty("IsActive")?.SetValue(movement, bool.Parse(parts[6]));
            typeof(BaseEntity).GetProperty("CreatedAt")?.SetValue(movement, DateTime.Parse(parts[7]));

            return movement;
        }

        private string[] SplitCsvLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result.ToArray();
        }

        private string FormatProductLine(Product product)
        {
            return $"{product.Id}," +
                   $"{product.Name}," +
                   $"{product.Price}," +
                   $"{(int)product.Category}," +
                   $"{product.Quantity}," +
                   $"{product.IsActive}," +
                   $"{product.CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }

        private string FormatMovementLine(StockMovement movement)
        {
            return $"{movement.Id}," +
                   $"{movement.ProductId}," +
                   $"{movement.QuantityChange}," +
                   $"{(int)movement.MovementType}," +
                   $"{movement.Reason}," +
                   $"{movement.Date:yyyy-MM-dd HH:mm:ss}," +
                   $"{movement.IsActive}," +
                   $"{movement.CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }
        #endregion 

        public void DeleteAllData()
        {
            try
            {
                if (File.Exists(_productsFilePath))
                {
                    File.Delete(_productsFilePath);
                    Console.WriteLine("[INFO] Deleted products file");
                }

                if (File.Exists(_movementsFilePath))
                {
                    File.Delete(_movementsFilePath);
                    Console.WriteLine("[INFO] Deleted movements file");
                }

                InitializeFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to delete data files: {ex.Message}");
            }
        }
    }
}
