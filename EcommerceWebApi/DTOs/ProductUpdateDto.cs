namespace EcommerceWebApi.DTOs
{
    public class ProductUpdateDto
    {
        public required string Name { get; set; }
        public required float Price { get; set; }
        public required string Description { get; set; }
        public required int Stock { get; set; }
        public required int CategoryId { get; set; }
    }
}
