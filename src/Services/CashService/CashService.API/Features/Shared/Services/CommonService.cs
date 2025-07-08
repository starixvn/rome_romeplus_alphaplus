using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand);

    Task WriteLogDBCommand(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel);

    Task<SetupCheckListModel> GetSetupCheckList(int custId);
}

public class CommonService(IDynamicRepository repository, Fanex.Logging.ILogger logger) : ICommonService
{
    public async Task<IEnumerable<string>> GetCountryCodes(string siteName)
    {
        var criteria = new GetCountryCodesCriteria(siteName);
        var result = await repository.FetchAsync<string>(criteria);
        return result;
    }

    public async Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand)
    {
        var writeLLogDBCommandModels = await repository.FetchAsync<WriteLogDBCommandModel>(baseDBCommand);

        if (baseDBCommand.ErrCode != 0)
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
        var isSuccess = await WriteLogDBCommandIntoDBLog(request, writeLogDBCommandModel);

        if (isSuccess)
        {
            return;
        }

        await WriteLogDBCommandIntoSiteDB(request, writeLogDBCommandModel);
    }

    public async Task<SetupCheckListModel> GetSetupCheckList(int custId)
    {
        var criteria = new GetSetupCheckListCriteria(custId);
        var result = await repository.GetAsync<SetupCheckListModel>(criteria);
        return result;
    }

    public async Task<bool> WriteLogDBCommandIntoDBLog(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        var isSuccess = false;

        try
        {
            var criteria = new WriteLogDBCommandCriteria(siteDB: -1, request, writeLogDBCommandModel);

            await repository.ExecuteAsync(criteria);
            isSuccess = true;
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e.Message, e);
        }

        return isSuccess;
    }

    public async Task WriteLogDBCommandIntoSiteDB(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        try
        {
            var criteria = new WriteLogDBCommandCriteria(siteDB: request.CustDBGroupId, request, writeLogDBCommandModel);
            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e.Message, e);
        }
    }
}