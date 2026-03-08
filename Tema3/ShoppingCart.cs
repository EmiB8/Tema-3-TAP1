using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce
{
    public class ShoppingCart
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

        public decimal SubTotal => _items.Sum(i => i.Subtotal);

        public bool IsEmpty => _items.Count == 0;

        public void AddItem(Product product, int quantity = 1)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentException("Cantitatea trebuie sa fie pozitiva.");
            if (quantity > product.Stock)
            {
                Console.WriteLine($"  [Cos] Stoc insuficient! Disponibil: {product.Stock}");
                return;
            }

            var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _items.Add(new CartItem(product, quantity));

            Console.WriteLine($"  [Cos] '{product.Name}' x{quantity} adaugat. ✓");
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
                Console.WriteLine($"  [Cos] '{item.Product.Name}' eliminat din cos.");
            }
        }

        public void Clear() => _items.Clear();

        public void DisplayCart()
        {
            if (IsEmpty)
            {
                Console.WriteLine("  Cosul este gol.");
                return;
            }
            Console.WriteLine("\n  === COS DE CUMPARATURI ===");
            foreach (var item in _items)
                Console.WriteLine($"  {item}");
            Console.WriteLine($"  Subtotal: {SubTotal:C}");
            Console.WriteLine("  ==========================\n");
        }
    }
}