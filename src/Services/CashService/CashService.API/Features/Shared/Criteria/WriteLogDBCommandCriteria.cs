using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class WriteLogDBCommandCriteria : NonQueryCommand
{
    public WriteLogDBCommandCriteria(int custDbGroupId, BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        CustDbGroupId = custDbGroupId;
        CustId = request.ActorId;
        UserId = request.UserId;
        CustDbGroupId = request.CustDBGroupId;

        ActionId = writeLogDBCommandModel.ActionId;
        TrackingValue = writeLogDBCommandModel.TrackingValue;
        TrackingValue2 = writeLogDBCommandModel.TrackingValue2;
        TrackingValueMask = writeLogDBCommandModel.TrackingValueMask;
        TrackingValueMask2 = writeLogDBCommandModel.TrackingValueMask2;
        Duration = writeLogDBCommandModel.Duration;
    }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; }

    public int CustId { get; }
    public int UserId { get; }
    public int ActionId { get; }
    public string TrackingValue { get; }
    public string TrackingValue2 { get; }
    public string TrackingValueMask { get; set; }
    public string TrackingValueMask2 { get; set; }
    public string Duration { get; }

    public override string GetSettingKey()
        => CustDbGroupId < 0 ? "WriteLog_CashService_UserLog_RunSP" : $"SiteDB_{CustDbGroupId}_WriteLog_CashService_UserLog_RunSP";

    public override bool IsValid() => CustId > 0 && ActionId > 0;
}