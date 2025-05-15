using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.DTOs;
using AlphaPlus.CashService.API.Features.Shared.Request;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<SiteDbConfig> GetServerDBConfigByCustIdAsync(int custId);

    Task WriteLogUserRunSP(int actionId, BaseActionRequest request, BaseActionCommand baseActionCommand);
}

public class CommonService(IDynamicRepository repository, Fanex.Logging.ILogger logger) : ICommonService
{
    public async Task<IEnumerable<string>> GetCountryCodes(string siteName)
    {
        var criteria = new GetCountryCodesCriteria(siteName);
        var result = await repository.FetchAsync<string>(criteria);
        return result;
    }

    public async Task<SiteDbConfig> GetServerDBConfigByCustIdAsync(int custId)
    {
        var criteria = new GetServerDBConfigByCustIdCriteria { CustId = custId };
        var siteDbConfig = await repository.GetAsync<SiteDbConfig>(criteria);
        return siteDbConfig;
    }

    public async Task WriteLogUserRunSP(int actionId, BaseActionRequest request, BaseActionCommand baseActionCommand)
    {
        if(baseActionCommand.ErrCode != 0)
        {
            return;
        }

        var isSuccess = false;

        try
        {
            var criteria = new WriteLogUserRunSPCriteria(actionId, request, baseActionCommand);
            await repository.ExecuteAsync(criteria);
            isSuccess = true;
        }
        catch (Exception e)
        {
            logger.Error(e.Message, e);
        }

        if(isSuccess)
        {
            return;
        }

        try
        {
            var criteria = new WriteLogUserRunSPCriteria(actionId, request, baseActionCommand);
            criteria.SiteDB = 0;
            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            logger.Error(e.Message, e);
        }
    }
}