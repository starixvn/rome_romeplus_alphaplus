using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.DTOs;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<SiteDbConfig> GetServerDBConfigByCustIdAsync(int custId);

    Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand);

    Task WriteLogDBCommand(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel);
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

    public async Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand)
    {
        var writeLLogDBCommandModels = await repository.FetchAsync<WriteLogDBCommandModel>(baseDBCommand);
        
        if(baseDBCommand.ErrCode != 0)
        {
            return new ResultModel(baseDBCommand.ErrCode, baseDBCommand.ErrMsg);
        }

        foreach (var writeLogDBCommandModel in writeLLogDBCommandModels)
        {
            await WriteLogDBCommand(request, writeLogDBCommandModel);
        }

        return new ResultModel(0, string.Empty);
    }

    public async Task WriteLogDBCommand(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        var isSuccess = false;

        try
        {
            var criteria = new WriteLogDBCommandCriteria(request, writeLogDBCommandModel);
            criteria.SiteDB = -1;

            await repository.ExecuteAsync(criteria);
            isSuccess = true;
        }
        catch (Exception e)
        {
            logger.Error(e.Message, e);
        }

        if(isSuccess)
        {
            // return;
        }

        try
        {
            var criteria = new WriteLogDBCommandCriteria(request, writeLogDBCommandModel);
            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            logger.Error(e.Message, e);
        }
    }
}