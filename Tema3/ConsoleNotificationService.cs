using System;

namespace ECommerce
{
    public class ConsoleNotificationService : INotificationService
    {
        public void NotifyOrderPlaced(Order order)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Comanda #{order.Id} plasata cu succes!");
            Console.WriteLine($"  Un email de confirmare a fost trimis.");
            Console.ResetColor();
        }
    }
}