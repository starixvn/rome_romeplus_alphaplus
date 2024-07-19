using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Criteria;
using AlphaPlus.Settings.Transfer.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ITransferService
{
    bool IsAllDayBalanceTransfer(int custId);

    Task<bool> IsAllDayBalanceTransferAsync(int custId);

    Task<string> GetAllowTransfer(int custId);

    Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId);

    Task<string> GetTransferOption(int custId);
}

public class TransferService(IDynamicRepository repository) : ITransferService
{
    private const int NumberDaysInWeek = 7;
    private static readonly string[] DaysInWeek =
    [
        Resources.Sunday,
        Resources.Monday,
        Resources.Tuesday,
        Resources.Wednesday,
        Resources.Thursday,
        Resources.Friday,
        Resources.Saturday
    ];
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

    public async Task<string> GetAllowTransfer(int custId)
    {
        var (isDaily, days) = await GetTransferCondition(custId);
        var transferCondition = isDaily ? Resources.Daily : string.Join(" - ", days);

        return Resources.AllowTransfer + (string.IsNullOrEmpty(transferCondition) ? string.Empty : " " + transferCondition);
    }

    public async Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId)
    {
        var transferOption = await GetTransferOption(custId);
        var charArray = transferOption.ToCharArray();

        if (charArray.Length == 0)
        {
            return Tuple.Create(false, Enumerable.Empty<string>());
        }

        if (charArray[NumberDaysInWeek] != '1')
        {
            return Tuple.Create(true, Enumerable.Empty<string>());
        }

        var dayBuilder = new List<string>();
        for (int i = 1, max = charArray.Length; i < max; i++)
        {
            var id = i % NumberDaysInWeek;

            if (charArray[id] == '1')
            {
                dayBuilder.Add(DaysInWeek[id]);
            }
        }

        return Tuple.Create<bool, IEnumerable<string>>(false, dayBuilder);
    }

    public async Task<string> GetTransferOption(int custId)
    {
        var criteria = new GetTransferOptionCriteria(custId);
        await repository.ExecuteAsync(criteria);

        return criteria.TransferOption;
    }
}
