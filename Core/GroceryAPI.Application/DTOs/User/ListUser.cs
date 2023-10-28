namespace GroceryAPI.Application.DTOs.User
{
    public class ListUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool TwoFactor { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }
}
