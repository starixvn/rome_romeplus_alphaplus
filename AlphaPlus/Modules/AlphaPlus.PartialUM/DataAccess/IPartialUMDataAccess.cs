using AlphaPlus.PartialUM.Models;

namespace AlphaPlus.PartialUM.DataAccess;

public interface IPartialUMDataAccess
{
    Task<IEnumerable<UMEntity>> GetUMEntities(string siteName);
}
