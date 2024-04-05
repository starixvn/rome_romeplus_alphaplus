namespace AlphaPlus.Features.Messages.Models.Product
{
    public class Product
    {
        public Product()
        {
        }

        public Product(int id, string name, bool isHidden)
        {
            Id = id;
            Name = name;
            IsHidden = isHidden;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsHidden { get; set; }
    }
}