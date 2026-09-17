using EcommerceWebApi.Models;
using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductCreateDto product);
        Task UpdateAsync(int id, ProductUpdateDto product);
        Task PatchAsync(int id, ProductPatchDto product);
        Task DeleteAsync(int id);
    }
}
