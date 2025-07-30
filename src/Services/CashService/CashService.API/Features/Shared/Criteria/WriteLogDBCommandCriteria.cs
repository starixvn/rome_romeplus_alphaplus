using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class WriteLogDBCommandCriteria(
    int custDbGroupId,
    BaseActionRequest request,
    WriteLogDBCommandModel writeLogDbCommandModel)
    : NonQueryCommand
{
    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; } = custDbGroupId;

    public int CustId { get; } = request.ActorId;
    public int UserId { get; } = request.UserId;

    [SpParam(AllowNull = true)]
    public int? ActionId { get; } = writeLogDbCommandModel.ActionId;

    public string TrackingValue { get; } = writeLogDbCommandModel.TrackingValue;
    public string TrackingValue2 { get; } = writeLogDbCommandModel.TrackingValue2;
    public string TrackingValueMask { get; set; } = writeLogDbCommandModel.TrackingValueMask;
    public string TrackingValueMask2 { get; set; } = writeLogDbCommandModel.TrackingValueMask2;
    public string Duration { get; } = writeLogDbCommandModel.Duration;

    public override string GetSettingKey()
        => CustDbGroupId < 0 ? "WriteLog_CashService_UserLog_RunSP" : $"CustDb_{CustDbGroupId}_WriteLog_CashService_UserLog_RunSP";

    public override bool IsValid() => CustId > 0;
}