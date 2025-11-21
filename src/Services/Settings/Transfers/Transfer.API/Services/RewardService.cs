using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.Reward;
using AlphaPlus.Settings.Transfer.API.Helpers;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using AlphaPlus.Settings.Transfer.API.Params.Reward;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Fanex.Data.Repository;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface IRewardService
{
    Task<IEnumerable<RewardProgram>> GetRewardPrograms();

    Task<RewardPayment> GetRewardPayments(RewardParam param);

    Task<RewardResult> UpdateRewardPaymentStatus(UpdateRewardPaymentStatusParam param);

    Task<RewardDetail> GetRewardDetails(RewardParam param);
}

public class RewardService(IDynamicRepository repository) : IRewardService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<IEnumerable<RewardProgram>> GetRewardPrograms()
    {
        var programs = await repository.FetchAsync<RewardProgram>(new GetRewardProgramsCriteria());

        return AssignProgramName(programs);
    }

    private IEnumerable<RewardProgram> AssignProgramName(IEnumerable<RewardProgram> programs)
    {
        foreach (var item in programs)
        {
            item.Name = CacheRefServiceHelper.GetProgramName(item.ResourceId);
            yield return item;
        }
    }

    public async Task<RewardPayment> GetRewardPayments(RewardParam param)
    {
        var total = new RewardPaymentEntity();
        var rawData = await repository.FetchAsync<RewardPaymentEntity>(BuildRewardPaymentCriteria(param));

        foreach (var item in rawData)
        {
            item.ProgramName = CacheRefServiceHelper.GetProgramName(item.ProgramId);
            total += item;
        }

        return new RewardPayment(rawData, total);
    }

    private static GetRewardCriteria BuildRewardPaymentCriteria(RewardParam param)
    {
        if (param.CustLevel.IsSuperLevel())
        {
            return new GetRewardPaymentMasterCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                param.ProgramIds,
                param.PaymentStatus,
                param.LevelType,
                param.Username);
        }

        if (param.CustLevel.IsMasterLevel())
        {
            return new GetRewardPaymentAgentCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                param.ProgramIds,
                param.PaymentStatus,
                param.LevelType,
                param.Username);
        }

        return new GetRewardPaymentMemberCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            param.ProgramIds,
            param.PaymentStatus,
            param.Username);
    }

    public async Task<RewardResult> UpdateRewardPaymentStatus(UpdateRewardPaymentStatusParam param)
    {
        var errors = new List<string>();

        foreach (var item in param.Data)
        {
            var result = await UpdateRewardPaymentStatus(item, param);

            if (result.IsFailed)
            {
                errors.Add($"{CacheRefServiceHelper.GetProgramName(item.ProgramId)} - {item.Username}: {result.Message}");
            }
        }

        return new RewardResult(param.Data.Length, errors);
    }

    private async Task<Result> UpdateRewardPaymentStatus(RewardPaymentStatus data, UpdateRewardPaymentStatusParam param)
    {
        const int AlreadyPaidError = 101;
        const string SqlDateFormat = "yyyy-MM-dd";
        var criteria = new UpdateRewardPaymentStatusCriteria(
            JsonConvert.SerializeObject(new
            {
                FromDate = param.FromDate.ToString(SqlDateFormat),
                ToDate = param.ToDate.ToString(SqlDateFormat),
                data.CustId,
                data.ProgramId,
                data.MaxTransId
            }, Formatting.None),
            param.ActorId,
            param.SubAccId,
            param.SubAccName,
            param.IP,
            param.Language)
        {
            SiteDbGroupId = param.SiteDbGroupId
        };
        await repository.ExecuteAsync(criteria);
        var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };

        if (result.IsSuccessful)
        {
            await LogRewardPaymentStatus(criteria.PaymentLog);
        }
        else if (result.Code == AlreadyPaidError)
        {
            result.Message = Resources.AlreadyPaid;
        }

        return result;
    }

    private async Task LogRewardPaymentStatus(string paymentLog)
    {
        try
        {
            await repository.ExecuteAsync(new RewardConfirmPaymentLogCriteria(paymentLog));
        }
        catch (Exception ex)
        {
            await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
        }
    }

    public async Task<RewardDetail> GetRewardDetails(RewardParam param)
    {
        var criteria = new GetRewardDetailCriteria(
            param.CustId,
            param.CustLevel,
            param.FromDate,
            param.ToDate,
            param.ProgramIds,
            param.PaymentStatus,
            param.Username,
            param.PageIndex,
            param.PageSize);
        var rawData = await repository.FetchAsync<RewardDetailEntity>(criteria);

        foreach (var item in rawData)
        {
            item.ProgramName = CacheRefServiceHelper.GetProgramName(item.ProgramId);
        }

        return new RewardDetail(rawData, param.PageIndex, param.PageSize, criteria.TotalRecords);
    }
}