using AlphaPlus.Products.Configurations;

namespace AlphaPlus.Products.Models
{
    public interface IRequest
    {
        ICustomer Actor { get; }

        Func<ProductItem, bool> Predicate { get; }

        bool IsValid { get; }

        Func<int, string> GetProductGroupName { get; }

        Func<int, string> GetProductName { get; }

        Func<string, string> GetResource { get; }

    }
}