namespace AlphaPlus.Reports.Features.Shared.Models;

public record ProductGroupDTO(int Id, string Name, bool Checked, IEnumerable<ProductGroupDTO> Products);
