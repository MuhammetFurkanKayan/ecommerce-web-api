using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        public ProductService(IGenericRepository<Product> repo) 
        {
            _repository = repo;
        }
        public async Task<Product> CreateAsync(ProductCreateDto product)
        {
            if(product.Price < 0.0f)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }
            if(product.Stock < 0)
            {
                throw new ArgumentException("Product stock cannot be negative.");
            }
            if(product.CategoryId <= 0)
            {
                throw new ArgumentException("Product category ID is invalid.");
            }
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };
            await _repository.AddAsync(newProduct);
            await _repository.SaveAsync();
            return newProduct;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            await _repository.DeleteAsync(product);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            } 
            return product;
        }

        public async Task PatchAsync(int id,ProductPatchDto product)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            if(product.Name != null)
            {
                existingProduct.Name = product.Name;
            }
            if(product.Description != null)
            {
                existingProduct.Description = product.Description;
            }
            if(product.Price.HasValue)
            {
                if(product.Price.Value < 0.0f)
                {
                    throw new ArgumentException("Product price cannot be negative.");
                }
                existingProduct.Price = product.Price.Value;
            }
            if(product.Stock.HasValue)
            {
                if(product.Stock.Value < 0)
                {
                    throw new ArgumentException("Product stock cannot be negative.");
                }
                existingProduct.Stock = product.Stock.Value;
            }
            if(product.CategoryId.HasValue)
            {
                if(product.CategoryId.Value <= 0)
                {
                    throw new ArgumentException("Product category ID is invalid.");
                }
                existingProduct.CategoryId = product.CategoryId.Value;
            }

            await _repository.UpdateAsync(existingProduct);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(int id,ProductUpdateDto product)
        {
            var exsistingProduct = await _repository.GetByIdAsync(id);

            if (exsistingProduct == null)
            { 
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            if (product.Price<0.0f)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }
            if (product.Stock < 0)
            {
                throw new ArgumentException("Product stock cannot be negative.");
            }
            if(product.CategoryId<=0)
            {
                throw new ArgumentException("Product category ID is invalid.");
            }
            exsistingProduct.Name = product.Name;
            exsistingProduct.Description = product.Description;
            exsistingProduct.Price = product.Price;
            exsistingProduct.Stock = product.Stock;
            exsistingProduct.CategoryId = product.CategoryId;

            await _repository.UpdateAsync(exsistingProduct);
            await _repository.SaveAsync();
        }
    }
}
