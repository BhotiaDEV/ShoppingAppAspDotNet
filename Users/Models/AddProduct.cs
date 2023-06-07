namespace Users.Models
{
    public class AddProduct
    {
        public String title { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public decimal discountPercentage { get; set; }
        public decimal rating { get; set; }
        public int stock { get; set; }
        public String brand { get; set; }
        public string category { get; set; }
        public String thumbnail { get; set; }
        public List<string> images { get; set; }
    }
}
