namespace AlphaPlus.Products.ViewModels
{
    public class ProductViewModel(int id, string name) : IViewModel
    {
        protected int Id { get; } = id;

        public string Name { get; } = name;

        public int DisplayOrder => Id;

        public bool Visible => true;

        public override string ToString()
        {
            return $"{{Id:{Id}, Name:{Name}}}";
        }
    }
}