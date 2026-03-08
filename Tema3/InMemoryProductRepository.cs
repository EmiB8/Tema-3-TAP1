using System.Collections.Generic;
using System.Linq;

namespace ECommerce
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public InMemoryProductRepository()
        {
            _products = new List<Product>
            {
                new Product(1,  "Laptop Dell XPS 15",        "Laptop premium 15 inch, Intel i7, 16GB RAM, 512GB SSD", 5299.99m, 10, "Electronice"),
                new Product(2,  "Mouse Logitech MX Master",   "Mouse wireless ergonomic",                               299.99m, 25, "Electronice"),
                new Product(3,  "Tastatura Mecanica Keychron", "Tastatura mecanica wireless, switch Brown",              599.99m, 15, "Electronice"),
                new Product(4,  "Monitor LG 27 4K",           "Monitor IPS 27 inch, 4K, 144Hz",                       2199.99m,  8, "Electronice"),
                new Product(5,  "Tricou Polo Clasic",         "Tricou polo din bumbac 100%",                             89.99m, 50, "Imbracaminte"),
                new Product(6,  "Jeans Slim Fit",             "Blugi slim fit, material premium",                       199.99m, 30, "Imbracaminte"),
                new Product(7,  "Rucsac Laptop 15 inch",      "Rucsac impermeabil cu multiple compartimente",           179.99m, 20, "Accesorii"),
                new Product(8,  "Casti Sony WH-1000XM5",     "Casti over-ear cu noise cancelling",                    1399.99m, 12, "Electronice"),
                new Product(9,  "Carte: Clean Code",          "Robert C. Martin - Clean Code",                           79.99m, 40, "Carti"),
                new Product(10, "Carte: Design Patterns",     "Gang of Four - Design Patterns",                          89.99m, 35, "Carti"),
            };
        }

        public List<Product> GetAll() => new List<Product>(_products);

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public bool UpdateStock(int productId, int quantity)
        {
            var product = GetById(productId);
            if (product == null || product.Stock < quantity) return false;
            product.Stock -= quantity;
            return true;
        }
    }
}