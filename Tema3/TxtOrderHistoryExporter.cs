using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ECommerce
{
    public class TxtOrderHistoryExporter : IOrderHistoryExporter
    {
        private readonly string _filePath;

        public string ExporterName => "Fisier TXT";

        public TxtOrderHistoryExporter(string filePath = "order_history.txt")
        {
            _filePath = filePath ?? "order_history.txt";
        }

        public void Export(List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                Console.WriteLine("  [Export] Nu exista comenzi de exportat.");
                return;
            }

            var sb = new StringBuilder();

            sb.AppendLine("================================================================");
            sb.AppendLine("         ISTORIC CUMPARATURI - E-COMMERCE SYSTEM               ");
            sb.AppendLine("================================================================");
            sb.AppendLine($"  Generat la: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"  Total comenzi: {orders.Count}");
            sb.AppendLine();

            decimal grandTotal = 0m;

            foreach (var order in orders)
            {
                sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine($"  COMANDA #{order.Id}");
                sb.AppendLine("----------------------------------------------------------------");
                sb.AppendLine($"  Client:          {order.CustomerName}");
                sb.AppendLine($"  Adresa livrare:  {order.ShippingAddress}");
                sb.AppendLine($"  Data comenzii:   {order.CreatedAt:dd/MM/yyyy HH:mm}");
                sb.AppendLine($"  Status:          {order.Status}");
                sb.AppendLine();
                sb.AppendLine("  Produse comandate:");
                foreach (var item in order.Items)
                    sb.AppendLine($"    - {item.Product.Name} x{item.Quantity} @ {item.Product.Price:C} = {item.Subtotal:C}");
                sb.AppendLine();
                sb.AppendLine($"  Subtotal:        {order.SubTotal:C}");
                sb.AppendLine($"  Reducere:       -{order.DiscountAmount:C}");
                sb.AppendLine($"  Transport:      +{order.ShippingCost:C}");
                sb.AppendLine($"  TOTAL PLATIT:    {order.Total:C}");
                sb.AppendLine();
                sb.AppendLine($"  Metoda de plata: {order.PaymentMethod}");
                sb.AppendLine($"  Curier:          {order.CourierCompany}");
                sb.AppendLine();

                grandTotal += order.Total;
            }

            sb.AppendLine("================================================================");
            sb.AppendLine($"  TOTAL CHELTUIT (toate comenzile): {grandTotal:C}");
            sb.AppendLine("================================================================");

            File.WriteAllText(_filePath, sb.ToString(), Encoding.UTF8);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [Export] Salvat in: {Path.GetFullPath(_filePath)}");
            Console.WriteLine($"  [Export] {orders.Count} comanda/comenzi | Total: {grandTotal:C}");
            Console.ResetColor();
        }
    }
}