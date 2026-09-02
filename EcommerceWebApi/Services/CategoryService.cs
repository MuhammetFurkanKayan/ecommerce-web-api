using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;

namespace EcommerceWebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        public CategoryService(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _repository.AddAsync(category);
            await _repository.SaveAsync();
            return category;
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
        

        public async Task PatchAsync(int id, Category category)
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

        public async Task UpdateAsync(int id, Category category)
        {
            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            if (category.Name == null)
            {
                throw new ArgumentException("Category name cannot be null.");
            }

            existingCategory.Name = category.Name;

            await _repository.UpdateAsync(existingCategory);
            await _repository.SaveAsync();
        }
    }
}
