using System;
using System.Collections.Generic;

namespace ECommerce
{
    class Program
    {
        static IProductRepository _productRepo = new InMemoryProductRepository();
        static INotificationService _notificationService = new ConsoleNotificationService();
        static IOrderService _orderService = new OrderService(_productRepo, _notificationService);
        static ShoppingCart _cart = new ShoppingCart();
        static IOrderHistoryExporter _exporter = new TxtOrderHistoryExporter("order_history.txt");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PrintBanner();

            bool running = true;
            while (running)
            {
                PrintMainMenu();
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;

                switch (choice)
                {
                    case "1": ShowCatalog(); break;
                    case "2": AddToCart(); break;
                    case "3": _cart.DisplayCart(); break;
                    case "4": RemoveFromCart(); break;
                    case "5": Checkout(); break;
                    case "6": ShowOrderHistory(); break;
                    case "7": ExportOrderHistory(); break;
                    case "0":
                        running = false;
                        Console.WriteLine("\nMultumim ca ai cumparat! La revedere!\n");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida. Incearca din nou.");
                        break;
                }
            }
        }

        static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================================");
            Console.WriteLine("        E-COMMERCE ORDER SYSTEM                 ");
            Console.WriteLine("        Aplicarea Principiilor SOLID             ");
            Console.WriteLine("================================================");
            Console.ResetColor();
        }

        static void PrintMainMenu()
        {
            Console.WriteLine("\n========== MENIU PRINCIPAL ==========");
            Console.WriteLine("  1. Catalog produse");
            Console.WriteLine("  2. Adauga in cos");
            Console.WriteLine("  3. Vezi cosul");
            Console.WriteLine("  4. Elimina din cos");
            Console.WriteLine("  5. Finalizeaza comanda (Checkout)");
            Console.WriteLine("  6. Istoric comenzi");
            Console.WriteLine("  7. Salveaza istoricul in fisier TXT");
            Console.WriteLine("  0. Iesire");
            Console.Write("Alegerea ta: ");
        }

        static void ShowCatalog()
        {
            Console.WriteLine("\n========== CATALOG PRODUSE ==========");
            var products = _productRepo.GetAll();
            foreach (var p in products)
                Console.WriteLine($"  {p}");
            Console.WriteLine("=====================================");
        }

        static void AddToCart()
        {
            ShowCatalog();
            Console.Write("\nID produs: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { Console.WriteLine("ID invalid."); return; }

            var product = _productRepo.GetById(id);
            if (product == null)
            { Console.WriteLine("Produsul nu exista."); return; }

            Console.Write($"Cantitate (disponibil: {product.Stock}): ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            { Console.WriteLine("Cantitate invalida."); return; }

            _cart.AddItem(product, qty);
        }

        static void RemoveFromCart()
        {
            if (_cart.IsEmpty)
            {
                Console.WriteLine("\nCosul este gol.");
                return;
            }

            Console.WriteLine("\n  === COS DE CUMPARATURI ===");
            var items = new List<CartItem>(_cart.Items);
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"  {i + 1}. {items[i]}");
            Console.WriteLine("  ==========================");

            Console.Write("Numarul produsului de eliminat: ");
            if (!int.TryParse(Console.ReadLine(), out int nr) || nr < 1 || nr > items.Count)
            {
                Console.WriteLine("Numar invalid.");
                return;
            }

            _cart.RemoveItem(items[nr - 1].Product.Id);
        }

        static void Checkout()
        {
            if (_cart.IsEmpty)
            {
                Console.WriteLine("\nCosul tau este gol!");
                return;
            }

            _cart.DisplayCart();

            Console.Write("Numele tau: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Adresa de livrare: ");
            string address = Console.ReadLine() ?? string.Empty;

            IPaymentProcessor payment = ChoosePaymentMethod();
            IShippingProvider shipping = ChooseShippingProvider();
            IDiscountStrategy discount = ChooseDiscount();

            Console.WriteLine("\nSe proceseaza comanda...");

            try
            {
                var order = _orderService.PlaceOrder(
                    name, address,
                    new List<CartItem>(_cart.Items),
                    payment, shipping, discount
                );

                order.Print();
                _cart.Clear();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nEroare: {ex.Message}");
                Console.ResetColor();
            }
        }

        static IPaymentProcessor ChoosePaymentMethod()
        {
            var processors = new List<IPaymentProcessor>
            {
                new CreditCardPayment(),
                new PayPalPayment(),
                new CashOnDeliveryPayment(),
                new CryptoPayment()
            };

            Console.WriteLine("\n--- Metoda de plata ---");
            for (int i = 0; i < processors.Count; i++)
                Console.WriteLine($"  {i + 1}. {processors[i].Name}");
            Console.Write("Alegere: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= processors.Count)
                return processors[choice - 1];

            Console.WriteLine("Alegere invalida, se foloseste plata la livrare.");
            return new CashOnDeliveryPayment();
        }

        static IShippingProvider ChooseShippingProvider()
        {
            var providers = new List<IShippingProvider>
            {
                new FanCourierShipping(),
                new DPDShipping(),
                new GlsShipping(),
                new SameaDayShipping()
            };

            Console.WriteLine("\n--- Optiuni transport ---");
            for (int i = 0; i < providers.Count; i++)
            {
                decimal cost = providers[i].CalculateShippingCost(new List<CartItem>(_cart.Items), "");
                Console.WriteLine($"  {i + 1}. {providers[i].Name} - {cost:C} ({providers[i].GetEstimatedDelivery()})");
            }
            Console.Write("Alegere: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= providers.Count)
                return providers[choice - 1];

            Console.WriteLine("Alegere invalida, se foloseste Fan Courier.");
            return new FanCourierShipping();
        }

        static IDiscountStrategy ChooseDiscount()
        {
            var campaigns = new List<IDiscountStrategy>
            {
                new NoDiscount(),
                new PercentageDiscount(10, "Reducere de Primavara"),
                new FixedAmountDiscount(50, "WELCOME50"),
                new MinimumOrderDiscount(500, 15),
                new BuyTwoGetOneFreeDiscount("Carti")
            };

            Console.WriteLine("\n--- Campanii de discount ---");
            for (int i = 0; i < campaigns.Count; i++)
            {
                decimal saved = campaigns[i].ApplyDiscount(_cart.SubTotal, new List<CartItem>(_cart.Items));
                string savings = saved > 0 ? $" (economisesti {saved:C})" : "";
                Console.WriteLine($"  {i + 1}. {campaigns[i].Name}{savings}");
            }
            Console.Write("Alegere: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= campaigns.Count)
                return campaigns[choice - 1];

            return new NoDiscount();
        }

        static void ShowOrderHistory()
        {
            var orders = _orderService.GetOrderHistory();
            if (orders.Count == 0)
            {
                Console.WriteLine("\nNu ai comenzi plasate inca.");
                return;
            }
            Console.WriteLine($"\n========== ISTORIC COMENZI ({orders.Count}) ==========");
            foreach (var order in orders)
                order.Print();
        }

        static void ExportOrderHistory()
        {
            var orders = _orderService.GetOrderHistory();
            if (orders.Count == 0)
            {
                Console.WriteLine("\nNu exista comenzi de exportat.");
                return;
            }
            Console.WriteLine($"\nSe exporteaza {orders.Count} comanda/comenzi via {_exporter.ExporterName}...");
            _exporter.Export(orders);
        }
    }
}