using System.ComponentModel.DataAnnotations.Schema;
using EcommerceWebApi.Enums;

namespace EcommerceWebApi.Models
{
  
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public float TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledAt { get; set; }
        public required int UserId { get; set; }
        public required User User { get; set; }

    }
}
