using EcommerceWebApi.Models;

namespace EcommerceWebApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task UpdateAsync(int id, Category category);
        Task PatchAsync(int id, Category category);
        Task DeleteAsync(int id);
    }
}
