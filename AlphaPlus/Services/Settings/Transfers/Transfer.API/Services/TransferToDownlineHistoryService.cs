using AlphaPlus.Settings.Transfer.API.Criteria.TransferToDownlineHistory;
using Fanex.Data.Repository;
using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Transfer.API.Services
{
    public interface TransferToDownlineHistoryService
    {
        Task<(IEnumerable<TransferToDownlineHistoryModel>, int)> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param);
    }

    public class TransferToDownlineHistoryServiceImpl(IDynamicRepository repository) : TransferToDownlineHistoryService
    {
        private readonly IDynamicRepository repository = repository;

        public async Task<(IEnumerable<TransferToDownlineHistoryModel>, int)> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
        {
            var criteria = new GetTransferToDownlineHistoryCriteria(
                param.CustId,
                param.UserName,
                param.FromDate,
                param.ToDate,
                param.PageSize,
                param.PageIndex);
            var transferToDownlines = await repository.FetchAsync<TransferToDownlineHistoryModel>(criteria);

            return (transferToDownlines, criteria.TotalRecords);
        }
    }
}