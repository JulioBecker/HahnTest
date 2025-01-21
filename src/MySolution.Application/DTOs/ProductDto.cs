namespace MySolution.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public float Rating { get; set; }
        public int RatingCount { get; set; }
    }
}
