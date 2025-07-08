using AgencyCash.API.Shared.Criteria;
using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Shared;

public interface IRequestLoggingService
{
    Task WriteLog(AddRequestLoggingCriteria criteria);
}

public class RequestLoggingService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : IRequestLoggingService
{
    private readonly IDynamicRepository repository = repository;
    private readonly ISiteDBService siteDBService = siteDBService;
    private readonly Fanex.Logging.ILogger logger = logger;

    public async Task WriteLog(AddRequestLoggingCriteria criteria)
    {
        var isSuccess = await ExecuteNonQuery(criteria);

        if (!isSuccess)
        {
            criteria.CustDBGroupId = criteria.UserId == 0
                ? (short)0
                : await siteDBService.GetCustDBGroupId(criteria.UserId);
            await ExecuteNonQuery(criteria);
        }
    }

    private async Task<bool> ExecuteNonQuery(INonQueryCommand criteria)
    {
        try
        {
            await repository.ExecuteAsync(criteria);
            return true;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"{nameof(WriteLog)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}", ex);
            return false;
        }
    }
}
