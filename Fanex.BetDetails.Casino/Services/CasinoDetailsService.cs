namespace Fanex.BetDetails.Casino.Services
{
    using BetList.Core.Entities;
    using DataAccesses;
    using Models;
    using Params;

    public class CasinoDetailsService : ICasinoDetailsService
    {
        private readonly ICasinoDetailsDataAccess casinoDetailDataAccess;

        public CasinoDetailsService(ICasinoDetailsDataAccess casinoDetailDataAccess)
        {
            this.casinoDetailDataAccess = casinoDetailDataAccess;
        }

        public PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param)
            => casinoDetailDataAccess.GetCasinoDetails(param);
            
        public PagedData<ITicket> GetLiveCasinoDetails(CasinoDetailsParam param)
            => casinoDetailDataAccess.GetLiveCasinoDetails(param);

        public PagedData<ITicket> GetRNGCasinoDetails(CasinoDetailsParam param)
            => casinoDetailDataAccess.GetRNGCasinoDetails(param);
    }
}
