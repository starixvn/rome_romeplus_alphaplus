using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class WriteLogDBCommandCriteria : NonQueryCommand
{
    public WriteLogDBCommandCriteria(BaseActionRequest request, WriteLogDBCommandModel writeLogDBCommandModel)
    {
        CustId = request.ActorId;
        UserId = request.UserId;
        SiteDB = request.SiteDB;
        
        ActionId = writeLogDBCommandModel.ActionId;
        TrackingValue = writeLogDBCommandModel.TrackingValue;
        TrackingValue2 = writeLogDBCommandModel.TrackingValue2;
    }

    public int CustId { get; }
    public int UserId { get; }
    public int ActionId { get; }
    public string TrackingValue { get; }
    public string TrackingValue2 { get; }

    [SpParam(Ignored = true)]
    public int SiteDB { get; set; }

    public override string GetSettingKey() => $"SiteDB_{SiteDB}_WriteLog_CashService_UserLog_RunSP";

    public override bool IsValid() => CustId > 0 && ActionId > 0;
} 