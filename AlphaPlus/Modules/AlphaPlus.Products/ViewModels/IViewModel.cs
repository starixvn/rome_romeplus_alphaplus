namespace AlphaPlus.Products.ViewModels
{
    public interface IViewModel
    {
        int DisplayOrder { get; }

        bool Visible { get; }
    }
}