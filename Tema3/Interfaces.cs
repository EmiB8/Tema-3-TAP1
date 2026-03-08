using System.Collections.Generic;

namespace ECommerce
{
    public interface IPaymentProcessor
    {
        string Name { get; }
        bool ProcessPayment(decimal amount, string customerName);
    }

    public interface IShippingProvider
    {
        string Name { get; }
        decimal CalculateShippingCost(List<CartItem> items, string address);
        string GetEstimatedDelivery();
    }

    public interface IDiscountStrategy
    {
        string Name { get; }
        decimal ApplyDiscount(decimal subTotal, List<CartItem> items);
    }

    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        bool UpdateStock(int productId, int quantity);
    }

    public interface IOrderService
    {
        Order PlaceOrder(
            string customerName,
            string address,
            List<CartItem> cartItems,
            IPaymentProcessor payment,
            IShippingProvider shipping,
            IDiscountStrategy discount
        );
        List<Order> GetOrderHistory();
    }

    public interface INotificationService
    {
        void NotifyOrderPlaced(Order order);
    }

    public interface IOrderHistoryExporter
    {
        string ExporterName { get; }
        void Export(List<Order> orders);
    }
}