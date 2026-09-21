using EcommerceWebApi.DTOs;
using EcommerceWebApi.Models;

namespace EcommerceWebApi.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> CreateAsync(OrderCreateDto order);
        Task UpdateStatusAsync(int id, UpdateOrderStatusDto status);
    }
}
