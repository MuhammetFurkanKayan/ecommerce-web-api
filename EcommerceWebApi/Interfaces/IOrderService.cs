using EcommerceWebApi.DTOs;
using EcommerceWebApi.Models;

namespace EcommerceWebApi.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order> GetByIdAsync(int id, int userId, bool isAdmin);
        Task<Order> CreateAsync(int userId, OrderCreateDto order);
        Task UpdateStatusAsync(int id, UpdateOrderStatusDto status);
        Task CancelAsync(int id, int userId, bool isAdmin);
        Task ShippingAddressUpdateAsync(int id, int userId, bool isAdmin, UpdateOrderShippingAddressDto address);
    }
}
