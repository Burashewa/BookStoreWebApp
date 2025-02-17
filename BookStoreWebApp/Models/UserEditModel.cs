namespace BookStoreWebApp.Models
{
    public class UserEditModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
    }
}
