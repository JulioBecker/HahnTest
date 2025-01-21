namespace MySolution.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string ImageUrl { get; private set; }

        public float Rating { get; private set; }
        public int RatingCount { get; private set; }

        public Product(int id,
                       string title,
                       decimal price,
                       string description,
                       string category,
                       string imageUrl,
                       float rating,
                       int ratingCount)
        {
            Id = id;
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            ImageUrl = imageUrl;
            Rating = rating;
            RatingCount = ratingCount;
        }

        public void Update(string title, decimal price, string description, string category, string imageUrl, float rating, int ratingCount)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            ImageUrl = imageUrl;
            Rating = rating;
            RatingCount = ratingCount;
        }
    }
}
