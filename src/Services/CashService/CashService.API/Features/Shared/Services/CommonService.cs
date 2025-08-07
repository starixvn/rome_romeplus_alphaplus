using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Features.Shared.Constants;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

using Newtonsoft.Json;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<ResultModel> RunDBCommand(BaseActionRequest request, BaseDBCommand baseDBCommand);

    Task WriteLogDBCommand(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel);

    Task<SetupCheckListModel> GetSetupCheckList(int custId);

    Task WriteLogRequestResponse(AddRequestLogCommand command, short custDbGroupId);
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

        var isSuccess = ErrorCodeConstants.IsSuccess(baseDBCommand.ErrCode) || ErrorCodeConstants.IsWarning(baseDBCommand.ErrCode);

        if (!isSuccess)
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

    public async Task WriteLogRequestResponse(AddRequestLogCommand command, short custDbGroupId)
    {
        var isSuccess = await ExecuteNonQuery(command);

        if (!isSuccess)
        {
            command.CustDBGroupId = custDbGroupId;
            await ExecuteNonQuery(command);
        }
    }

    private async Task<bool> ExecuteNonQuery(AddRequestLogCommand command)
    {
        try
        {
            await repository.ExecuteAsync(command);
            return true;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"{nameof(WriteLogRequestResponse)} failed. Key: {command.GetSettingKey()}, Criteria: {JsonConvert.SerializeObject(command)}", ex);
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