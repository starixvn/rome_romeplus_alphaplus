namespace Fanex.BetDetails.Casino.DataAccesses
{
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailsDataAccess
    {
        PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param);
        PagedData<ITicket> GetLiveCasinoDetails(CasinoDetailsParam param);
        PagedData<ITicket> GetRNGCasinoDetails(CasinoDetailsParam param);
    }
}
