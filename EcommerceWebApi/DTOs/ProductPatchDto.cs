namespace EcommerceWebApi.DTOs
{
    public class ProductPatchDto
    {
        public string? Name { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
    }
}
