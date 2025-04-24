namespace AlphaPlus.BetLists.Shared.Models;

public record ProductGroupDTO(int Id, string Name, bool Checked, IEnumerable<ProductGroupDTO> Products);
