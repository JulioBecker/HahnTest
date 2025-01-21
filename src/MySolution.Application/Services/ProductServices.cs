using MySolution.Domain.Entities;
using MySolution.Domain.Interfaces;
using MySolution.Application.DTOs;

namespace MySolution.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                Rating = p.Rating,
                RatingCount = p.RatingCount
            });
        }

        public async Task UpsertProductsAsync(IEnumerable<ProductDto> productsDto)
        {
            foreach (var dto in productsDto)
            {
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    // Insert
                    var newProduct = new Product(
                        dto.Id,
                        dto.Title,
                        dto.Price,
                        dto.Description,
                        dto.Category,
                        dto.ImageUrl,
                        dto.Rating,
                        dto.RatingCount
                    );
                    await _repository.AddAsync(newProduct);
                }
                else
                {
                    // Update
                    existing.Update(
                        dto.Title,
                        dto.Price,
                        dto.Description,
                        dto.Category,
                        dto.ImageUrl,
                        dto.Rating,
                        dto.RatingCount
                    );
                    await _repository.UpdateAsync(existing);
                }
            }
        }
    }
}
