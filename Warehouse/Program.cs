using Warehouse.Data;
using Warehouse.Repositories;
using Warehouse.Services;
using Warehouse.UI;

namespace Warehouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var csvDataManager = new CsvDataManager("Data");

            var productRepository = new ProductRepository(csvDataManager);
            var movementRepository = new StockMovementRepository(csvDataManager);

            var inventoryService = new InventoryService(productRepository, movementRepository);
            var menu = new Menu(inventoryService);

            menu.Start();
        }
    }
}