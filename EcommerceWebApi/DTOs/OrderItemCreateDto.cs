namespace EcommerceWebApi.DTOs
{
    public class OrderItemCreateDto
    {
        public required int Quantity { get; set; }
        public required int ProductId { get; set; }
    }
}
