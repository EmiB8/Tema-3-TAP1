using System;

namespace ECommerce
{
    public class CreditCardPayment : IPaymentProcessor
    {
        public string Name => "Card de Credit/Debit";
        public bool ProcessPayment(decimal amount, string customerName)
        {
            Console.WriteLine($"  [Card] Procesare plata {amount:C} pentru {customerName}...");
            Console.WriteLine("  [Card] Plata aprobata!");
            return true;
        }
    }

    public class PayPalPayment : IPaymentProcessor
    {
        public string Name => "PayPal";
        public bool ProcessPayment(decimal amount, string customerName)
        {
            Console.WriteLine($"  [PayPal] Redirectionare catre PayPal pentru {amount:C}...");
            Console.WriteLine("  [PayPal] Tranzactie finalizata cu succes!");
            return true;
        }
    }

    public class CashOnDeliveryPayment : IPaymentProcessor
    {
        public string Name => "Plata la livrare (Ramburs)";
        public bool ProcessPayment(decimal amount, string customerName)
        {
            Console.WriteLine($"  [Ramburs] Vei plati {amount:C} la livrare.");
            Console.WriteLine("  [Ramburs] Comanda inregistrata!");
            return true;
        }
    }

    public class CryptoPayment : IPaymentProcessor
    {
        public string Name => "Cryptocurrency (Bitcoin)";
        public bool ProcessPayment(decimal amount, string customerName)
        {
            Console.WriteLine($"  [Crypto] Generare adresa wallet pentru {amount:C}...");
            Console.WriteLine("  [Crypto] Tranzactie confirmata pe blockchain!");
            return true;
        }
    }
}