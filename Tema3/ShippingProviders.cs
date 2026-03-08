using System.Collections.Generic;

namespace ECommerce
{
    public class FanCourierShipping : IShippingProvider
    {
        public string Name => "Fan Courier";
        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return 15m + (totalItems * 2m);
        }
        public string GetEstimatedDelivery() => "1-2 zile lucratoare";
    }

    public class DPDShipping : IShippingProvider
    {
        public string Name => "DPD";
        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return 12m + (totalItems * 1.5m);
        }
        public string GetEstimatedDelivery() => "2-3 zile lucratoare";
    }

    public class GlsShipping : IShippingProvider
    {
        public string Name => "GLS";
        public decimal CalculateShippingCost(List<CartItem> items, string address)
        {
            int totalItems = 0;
            foreach (var item in items) totalItems += item.Quantity;
            return 10m + (totalItems * 1.8m);
        }
        public string GetEstimatedDelivery() => "2-4 zile lucratoare";
    }

    public class SameaDayShipping : IShippingProvider
    {
        public string Name => "Livrare Express (aceeasi zi)";
        public decimal CalculateShippingCost(List<CartItem> items, string address) => 35m;
        public string GetEstimatedDelivery() => "Astazi, pana la ora 22:00";
    }
}