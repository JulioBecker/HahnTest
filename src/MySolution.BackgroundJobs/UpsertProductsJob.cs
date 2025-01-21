using System.Net.Http.Json;
using MySolution.Application.DTOs;
using MySolution.Application.Services;

namespace MySolution.BackgroundJobs
{
    public class UpsertProductsJob
    {
        private readonly ProductService _productService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UpsertProductsJob(ProductService productService, IHttpClientFactory httpClientFactory)
        {
            _productService = productService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync("https://fakestoreapi.com/products");
                response.EnsureSuccessStatusCode();

                var productsFromApi = await response.Content.ReadFromJsonAsync<List<FakeStoreProductDto>>();

                if (productsFromApi is not null)
                {
                    var productDtos = productsFromApi.Select(p => new ProductDto
                    {
                        Id = p.id,
                        Title = p.title,
                        Price = Convert.ToDecimal(p.price),
                        Description = p.description,
                        Category = p.category,
                        ImageUrl = p.image,
                        Rating = p.rating?.rate ?? 0,
                        RatingCount = p.rating?.count ?? 0
                    });

                    await _productService.UpsertProductsAsync(productDtos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing upsert job: {ex.Message}");
            }
        }
    }

    public class FakeStoreProductDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string image { get; set; }
        public RatingDto rating { get; set; }
    }

    public class RatingDto
    {
        public float rate { get; set; }
        public int count { get; set; }
    }
}
