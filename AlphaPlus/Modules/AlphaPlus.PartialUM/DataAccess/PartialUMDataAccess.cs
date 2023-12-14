using AlphaPlus.PartialUM.Models;

namespace AlphaPlus.PartialUM.DataAccess
{
    public interface IPartialUMDataAccess
    {
        IEnumerable<UMInfo> GetUMInfoList(string siteName);
    }
}
