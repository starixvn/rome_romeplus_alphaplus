using AlphaPlus.CashService.API.Features.Shared.Request;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class WriteLogUserRunSPCriteria : NonQueryCommand
{
    public WriteLogUserRunSPCriteria(int actionId, BaseActionRequest request, BaseActionCommand baseActionCommand)
    {
        ActionId = actionId;
        CustId = request.ActorId;
        UserId = request.UserId;
        SiteDB = request.SiteDB;
        TrackingValue = baseActionCommand.TrackingValue;
        TrackingValue2 = baseActionCommand.TrackingValue2;
    }

    public int CustId { get; }
    public int UserId { get; }
    public int ActionId { get; }
    public string TrackingValue { get; }
    public string TrackingValue2 { get; }

    [SpParam(Ignored = true)]
    public int SiteDB { get; set; }

    public override string GetSettingKey() => $"SiteDB_{SiteDB}_WriteLog_UserLog_RunSP";

    public override bool IsValid() => CustId > 0 && ActionId > 0;
} 