namespace EcommerceWebApi.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public required string Address { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}