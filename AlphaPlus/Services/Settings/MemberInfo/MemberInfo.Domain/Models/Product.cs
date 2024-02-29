namespace AlphaPlus.Settings.MemberInfo.Domain.Models
{
    public class Product
    {
        public Product()
        {
        }

        public Product(byte id, string name, bool isDisable)
        {
            Id = id;
            Name = name;
            IsDisable = isDisable;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDisable { get; set; }
    }
}