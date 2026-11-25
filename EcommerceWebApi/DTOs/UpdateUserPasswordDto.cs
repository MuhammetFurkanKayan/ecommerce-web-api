namespace EcommerceWebApi.DTOs
{
    public class UpdateUserPasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}