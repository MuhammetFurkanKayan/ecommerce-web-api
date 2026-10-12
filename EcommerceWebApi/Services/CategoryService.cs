using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        public async Task<Category> CreateAsync(CategoryCreateDto category)
        {
            var newCategory = new Category
            {
                Name = category.Name
            };
            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveAsync();
            return newCategory;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            if (await _productRepository.AnyAsync(p => p.CategoryId == id))
            {
                throw new InvalidOperationException("Category has products and cannot be deleted.");
            }
            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveAsync();
        }
        

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            return category;
        }
        

        public async Task PatchAsync(int id, CategoryPatchDto category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            if (category.Name != null)
            {
                existingCategory.Name = category.Name;
            }

            await _categoryRepository.UpdateAsync(existingCategory);
            await _categoryRepository.SaveAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            existingCategory.Name = category.Name;

            await _categoryRepository.UpdateAsync(existingCategory);
            await _categoryRepository.SaveAsync();
        }
    }
}
