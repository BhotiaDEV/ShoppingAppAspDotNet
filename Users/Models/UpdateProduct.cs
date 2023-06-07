namespace Users.Models
{
    public class UpdateProduct
    {
        public String? title { get; set; }
        public String? description { get; set; }
        public decimal? price { get; set; }
        public decimal? discountPercentage { get; set; }
        public decimal? rating { get; set; }
        public int? stock { get; set; }
        public String? brand { get; set; }
        public String? category { get; set; }
        public String? thumbnail { get; set; }
        public List<String>? images { get; set; }
    }
}
