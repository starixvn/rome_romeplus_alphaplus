namespace Alpha.Main.DataAccesses
{
    using AlphaPlus.Core.Models;
    using Fanex.Data.Repository;

    public interface SecurityCodeDataAccess
    {
        string GetSecurityCode(ICriteria criteria);

        Result UpdateSecurityCode(ICriteria criteria);
    }
}