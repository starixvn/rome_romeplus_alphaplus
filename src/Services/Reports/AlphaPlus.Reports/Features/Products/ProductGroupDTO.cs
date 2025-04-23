namespace AlphaPlus.Reports.Features.Products;

public record ProductGroupDTO(int Id, string Name, bool Checked, IEnumerable<ProductGroupDTO> Products);
