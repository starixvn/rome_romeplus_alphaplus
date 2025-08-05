using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class ResetCurrentWinAmountCommand : IRequest<Result>
{
    public ResetCurrentWinAmountCommand() { }

    public ResetCurrentWinAmountCommand(IUser viewer)
    {   
        SubAccId = viewer.SubAccId;
        SubAccName = viewer.SubAccName;
        UserId = viewer.Id;
        IP = viewer.IP;
        SettingStatus = AppSettings.SettingStatus;
        Language = viewer.Language;
    }

    public int CustId { get; set; } 

    public int ProductId { get; set; }

    public int SportType { get; set; }

    public int BetType { get; set; }

    public int SubAccId { get; set; }

    public string SubAccName { get; set; }

    public int UserId { get; set; }

    public string IP { get; set; }

    public int SettingStatus { get; set; }

    public string Username { get; set; }

    public string Language { get; set; }
} 