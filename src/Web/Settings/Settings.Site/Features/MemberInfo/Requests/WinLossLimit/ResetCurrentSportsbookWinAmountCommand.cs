using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class ResetCurrentSportsbookWinAmountCommand : IRequest<Result>
{
    public ResetCurrentSportsbookWinAmountCommand() { }

    public ResetCurrentSportsbookWinAmountCommand(IUser viewer)
    {
        UserId = viewer.Id;
        Language = viewer.Language;
        SettingStatus = AppSettings.SettingStatus;
        SiteDB = (short)viewer.SiteDBGroupId;
    }

    public short SiteDB { get; set; }
    public int CustId { get; set; } 
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Language { get; set; }
    public int SettingStatus { get; set; }
} 