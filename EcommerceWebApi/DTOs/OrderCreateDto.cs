namespace EcommerceWebApi.DTOs
{
    public class OrderCreateDto
    {
        public required List<OrderItemCreateDto> Items { get; set; }
    }
}
