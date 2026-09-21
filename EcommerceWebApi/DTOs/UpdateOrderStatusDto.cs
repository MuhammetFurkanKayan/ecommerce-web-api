using EcommerceWebApi.Enums;

namespace EcommerceWebApi.DTOs
{
    public class UpdateOrderStatusDto
    {
        public required OrderStatus Status { get; set; }
    }
}
