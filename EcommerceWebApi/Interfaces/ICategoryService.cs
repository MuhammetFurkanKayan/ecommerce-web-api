using EcommerceWebApi.Models;
using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> CreateAsync(CategoryCreateDto category);
        Task UpdateAsync(int id, CategoryUpdateDto category);
        Task PatchAsync(int id, CategoryPatchDto category);
        Task DeleteAsync(int id);
    }
}
