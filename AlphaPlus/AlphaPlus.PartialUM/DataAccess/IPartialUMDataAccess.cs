namespace AlphaPlus.PartialUM.DataAccess
{
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Models;

    public interface IPartialUMDataAccess
    {
        IEnumerable<UMInfo> GetUMInfoList(string siteName);
    }
}
