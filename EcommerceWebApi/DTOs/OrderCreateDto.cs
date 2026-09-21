namespace EcommerceWebApi.DTOs
{
    public class OrderCreateDto
    {
        public required int UserId { get; set; }
        public required List<OrderItemCreateDto> Items { get; set; }
    }
}
