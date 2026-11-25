using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApi.DTOs
{
    public class UpdateUserEmailDto
    {
        public required string CurrentPassword { get; set; }
        [EmailAddress]
        public required string NewEmail { get; set; }
    }
}