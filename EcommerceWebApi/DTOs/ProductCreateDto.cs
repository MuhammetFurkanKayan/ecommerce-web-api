namespace EcommerceWebApi.DTOs
{
    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public float Price { get; set; }
        public required string Description { get; set; }
        public int Stock { get; set; }
        public required int CategoryId { get; set; }
    }
}
