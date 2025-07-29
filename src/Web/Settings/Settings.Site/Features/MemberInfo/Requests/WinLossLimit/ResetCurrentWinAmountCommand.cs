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
        UserId = viewer.Id;
        Language = viewer.Language;
        SettingStatus = AppSettings.SettingStatus;
    }

    public int CustId { get; set; } 
    public int UserId { get; set; }
    public string Username { get; set; }
    public int ProductId { get; set; }
    public int SportType { get; set; }
    public int BetType { get; set; }
    public string Language { get; set; }
    public int SettingStatus { get; set; }
} 