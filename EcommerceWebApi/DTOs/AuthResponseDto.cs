namespace EcommerceWebApi.DTOs
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Role { get; set; }
    }
}
