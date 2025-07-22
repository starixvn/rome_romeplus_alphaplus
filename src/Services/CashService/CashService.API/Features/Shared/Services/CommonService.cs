using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

using Newtonsoft.Json;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand);

    Task WriteLogDBCommand(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel);

    Task<SetupCheckListModel> GetSetupCheckList(int custId);

    Task WriteLogRequestResponse(AddRequestLogCommand requestLog, short custDbGroupId);
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

        await WriteLogDBCommandIntoCustDB(request, writeLogDBCommandModel);
    }

    public async Task<SetupCheckListModel> GetSetupCheckList(int custId)
    {
        var criteria = new GetSetupCheckListCriteria(custId);
        var result = await repository.GetAsync<SetupCheckListModel>(criteria);
        return result;
    }

    public async Task WriteLogRequestResponse(AddRequestLogCommand criteria, short custDbGroupId)
    {
        var isSuccess = await ExecuteNonQuery(criteria);

        if (!isSuccess)
        {
            criteria.CustDBGroupId = custDbGroupId;
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
            await logger.ErrorAsync($"{nameof(WriteLogRequestResponse)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonConvert.SerializeObject(criteria)}", ex);
            return false;
        }
    }

    public async Task<bool> WriteLogDBCommandIntoDBLog(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        var isSuccess = false;

        try
        {
            var criteria = new WriteLogDBCommandCriteria(custDbGroupId: -1, request, writeLogDBCommandModel);
            await repository.ExecuteAsync(criteria);
            isSuccess = true;
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e.Message, e);
        }

        return isSuccess;
    }

    public async Task WriteLogDBCommandIntoCustDB(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        try
        {
            var criteria = new WriteLogDBCommandCriteria(custDbGroupId: request.CustDBGroupId, request, writeLogDBCommandModel);
            await repository.ExecuteAsync(criteria);
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e.Message, e);
        }
    }
}