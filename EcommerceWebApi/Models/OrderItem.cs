using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebApi.Models
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public required int OrderId { get; set; }
        public required Order Order { get; set; }
        public required int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
