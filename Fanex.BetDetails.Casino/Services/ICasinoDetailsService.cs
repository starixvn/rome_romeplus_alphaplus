namespace Fanex.BetDetails.Casino.Services
{
    using BetList.Core.Entities;
    using Models;
    using Params;

    public interface ICasinoDetailsService
    {
        PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param);
        PagedData<ITicket> GetLiveCasinoDetails(CasinoDetailsParam param);
        PagedData<ITicket> GetRNGCasinoDetails(CasinoDetailsParam param);
    }
}
