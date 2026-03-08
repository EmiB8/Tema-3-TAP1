using System;
using System.Collections.Generic;

namespace ECommerce
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly INotificationService _notificationService;
        private readonly List<Order> _orderHistory = new List<Order>();
        private int _nextOrderId = 1001;

        public OrderService(IProductRepository productRepository, INotificationService notificationService)
        {
            _productRepository = productRepository;
            _notificationService = notificationService;
        }

        public Order PlaceOrder(
            string customerName,
            string address,
            List<CartItem> cartItems,
            IPaymentProcessor payment,
            IShippingProvider shipping,
            IDiscountStrategy discount)
        {
            if (cartItems == null || cartItems.Count == 0)
                throw new InvalidOperationException("Cosul este gol.");

            foreach (var item in cartItems)
            {
                if (!_productRepository.UpdateStock(item.Product.Id, item.Quantity))
                    throw new InvalidOperationException($"Stoc insuficient pentru '{item.Product.Name}'.");
            }

            decimal subTotal = 0m;
            foreach (var item in cartItems)
                subTotal += item.Subtotal;

            decimal discountAmount = discount.ApplyDiscount(subTotal, cartItems);
            decimal shippingCost = shipping.CalculateShippingCost(cartItems, address);
            decimal total = subTotal - discountAmount + shippingCost;

            Console.WriteLine($"\n  Procesare plata via {payment.Name}...");
            bool paymentSuccess = payment.ProcessPayment(total, customerName);
            if (!paymentSuccess)
                throw new InvalidOperationException("Plata a esuat.");

            var order = new Order(_nextOrderId++, customerName, address, new List<CartItem>(cartItems))
            {
                SubTotal = subTotal,
                DiscountAmount = discountAmount,
                ShippingCost = shippingCost,
                Total = total,
                PaymentMethod = payment.Name,
                CourierCompany = $"{shipping.Name} (Est: {shipping.GetEstimatedDelivery()})",
                Status = OrderStatus.Paid
            };

            _orderHistory.Add(order);
            _notificationService.NotifyOrderPlaced(order);

            return order;
        }

        public List<Order> GetOrderHistory() => new List<Order>(_orderHistory);
    }
}