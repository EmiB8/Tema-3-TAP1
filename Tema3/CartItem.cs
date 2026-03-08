
namespace ECommerce
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal Subtotal => Product.Price * Quantity;

        public override string ToString()
        {
            return $"{Product.Name} x{Quantity} = {Subtotal:C}";
        }
    }
}