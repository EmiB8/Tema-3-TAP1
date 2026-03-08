using System;
using System.Collections.Generic;

namespace ECommerce
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string CourierCompany { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;

        public Order(int id, string customerName, string shippingAddress, List<CartItem> items)
        {
            Id = id;
            CustomerName = customerName ?? string.Empty;
            ShippingAddress = shippingAddress ?? string.Empty;
            Items = items;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
        }

        public void Print()
        {
            Console.WriteLine($"\n========== COMANDA #{Id} ==========");
            Console.WriteLine($"Client: {CustomerName}");
            Console.WriteLine($"Adresa livrare: {ShippingAddress}");
            Console.WriteLine($"Data: {CreatedAt:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine("--- Produse ---");
            foreach (var item in Items)
                Console.WriteLine($"  {item}");
            Console.WriteLine($"Subtotal:    {SubTotal:C}");
            Console.WriteLine($"Reducere:   -{DiscountAmount:C}");
            Console.WriteLine($"Transport:  +{ShippingCost:C}");
            Console.WriteLine($"TOTAL:       {Total:C}");
            Console.WriteLine($"Plata:       {PaymentMethod}");
            Console.WriteLine($"Curier:      {CourierCompany}");
            Console.WriteLine("====================================\n");
        }
    }
}