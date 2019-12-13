namespace PayPalApi.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public ProductModel(long id, string name, bool isComplete) {
                Id = id;
                Name = name;
                IsComplete = isComplete;
        }
    }
}