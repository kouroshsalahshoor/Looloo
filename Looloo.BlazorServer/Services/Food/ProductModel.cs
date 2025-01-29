namespace Looloo.BlazorServer.Services.Food
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Price { get; set; }
        //public decimal Price { get; set; }
        public string? Size { get; set; }
        public string? SizePrice { get; set; }
        //public decimal SizePrice { get; set; }
        public string? Company { get; set; }
        public string? ImageUrl { get; set; }
        public string? SearchUrl { get; set; }

    }
}
