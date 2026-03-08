// ============================================================
// OCP - Open/Closed Principle:
// Adaugarea unui nou curier nu modifica codul existent.
// LSP - Liskov Substitution Principle:
// Orice IShippingProvider poate fi folosit interschimbabil.
// ============================================================

using System;
using System.Collections.Generic;
using ECommerce.Interfaces;
using ECommerce.Models;

namespace ECommerce.Shipping
{
    public class FanCourierShipping : IShippingProvider
    {
        public string Name => "Fan Courier";

        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            // Logica simplificata: cost fix + cost per kg estimat
            decimal baseCost = 15m;
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return baseCost + (totalItems * 2m);
        }

        public string GetEstimatedDelivery() => "1-2 zile lucratoare";
    }

    public class DPDShipping : IShippingProvider
    {
        public string Name => "DPD";

        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            decimal baseCost = 12m;
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return baseCost + (totalItems * 1.5m);
        }

        public string GetEstimatedDelivery() => "2-3 zile lucratoare";
    }

    public class SameaDayShipping : IShippingProvider
    {
        public string Name => "Livrare in aceeasi zi (Express)";

        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            return 35m; // Cost fix pentru livrare expres
        }

        public string GetEstimatedDelivery() => "Astazi, pana la ora 22:00";
    }

    // OCP: Nou curier adaugat fara a modifica nimic existent
    public class GlsShipping : IShippingProvider
    {
        public string Name => "GLS";

        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            decimal baseCost = 10m;
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return baseCost + (totalItems * 1.8m);
        }

        public string GetEstimatedDelivery() => "2-4 zile lucratoare";
    }
}