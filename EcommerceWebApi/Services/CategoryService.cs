using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        public CategoryService(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<Category> CreateAsync(CategoryCreateDto category)
        {
            var newCategory = new Category
            {
                Name = category.Name
            };
            await _repository.AddAsync(newCategory);
            await _repository.SaveAsync();
            return newCategory;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            await _repository.DeleteAsync(category);
            await _repository.SaveAsync();
        }
        

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            return category;
        }
        

        public async Task PatchAsync(int id, CategoryPatchDto category)
        {
            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            if (category.Name != null)
            {
                existingCategory.Name = category.Name;
            }

            await _repository.UpdateAsync(existingCategory);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto category)
        {
            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            existingCategory.Name = category.Name;

            await _repository.UpdateAsync(existingCategory);
            await _repository.SaveAsync();
        }
    }
}
