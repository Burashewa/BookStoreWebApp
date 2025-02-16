using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public bool isAdmin { get; set; } = false;
        public DateTime created_at { get; set; } = DateTime.Now;
        
        // Navigation properties
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
    }
}
