namespace BookStoreWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Category Category { get; set; }
    }

}
