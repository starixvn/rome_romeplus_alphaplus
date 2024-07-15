using AlphaPlus.Settings.Transfer.API.Criteria;
using AlphaPlus.Settings.Transfer.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface TransferService
{
    bool IsAllDayBalanceTransfer(int custId);

    Task<bool> IsAllDayBalanceTransferAsync(int custId);
}

public class TransferServiceImpl(IDynamicRepository repository) : TransferService
{
    private readonly IDynamicRepository repository = repository;

    public bool IsAllDayBalanceTransfer(int custId)
    {
        var transferModel = repository.Get<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

        return transferModel?.IsAllDayBalTransfer == true;
    }

    public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
    {
        var transferModel = await repository.GetAsync<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

        return transferModel?.IsAllDayBalTransfer == true;
    }
}
