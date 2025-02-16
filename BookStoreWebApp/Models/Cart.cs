namespace BookStoreWebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProId { get; set; }
        public string ProName { get; set; }
        public string ProImage { get; set; }
        public int ProPrice { get; set; }
        public int ProAmount { get; set; }
        public string ProFile { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public Users Users { get; set; }
    }
}
