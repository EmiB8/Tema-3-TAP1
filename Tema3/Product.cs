namespace ECommerce
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;

        public Product(int id, string name, string description, decimal price, int stock, string category)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Category = category;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Price:C} (Stoc: {Stock}) [{Category}]";
        }
    }
}