using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.Params;

public class CreditBalanceParam
{
    public CreditBalanceParam(
        int custId,
        UserLevel custLevel,
        string userName,
        byte statusId,
        byte levelType,
        int pageIndex,
        int pageSize,
        bool isAdminCanViewVNIP,
        string agIP,
        int sRecommendId,
        string system)
    {
        CustId = custId;
        CustLevel = custLevel;
        UserName = userName;
        MapStatusIdToParam(statusId);
        LevelType = levelType;
        PageIndex = pageIndex;
        PageSize = pageSize;
        IsAdminCanViewVNIP = isAdminCanViewVNIP;
        AGIP = agIP;
        SRecommendId = sRecommendId;
        System = system;
    }

    public int CustId { get; }

    public UserLevel CustLevel { get; }

    public string UserName { get; }

    public bool? Enabled { get; private set; }

    public bool? Closed { get; private set; }

    public bool? Suspended { get; private set; }

    public byte LevelType { get; }

    public int PageIndex { get; }

    public int PageSize { get; }

    public bool IsAdminCanViewVNIP { get; }

    public string AGIP { get; }

    public int SRecommendId { get; }

    public string System { get; }

    private void MapStatusIdToParam(byte statusId)
    {
        switch (statusId)
        {
            case StatusIdentity.Open:
                Closed = false;
                break;

            case StatusIdentity.Suspended:
                Suspended = true;
                break;

            case StatusIdentity.Closed:
                Closed = true;
                break;

            case StatusIdentity.Disabled:
                Enabled = false;
                break;

            case StatusIdentity.All:
            default:
                break;
        }
    }
}