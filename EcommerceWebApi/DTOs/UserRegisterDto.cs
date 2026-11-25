namespace EcommerceWebApi.DTOs
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
    }
}
