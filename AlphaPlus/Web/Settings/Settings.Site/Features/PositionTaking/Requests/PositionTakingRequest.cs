using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Requests;

public class PositionTakingRequest : IRequest<object>
{
    public int MainProductId { get; set; }

    public int ProductId { get; set; } = ProductIdConstants.Sportsbook;

    public int ProductGroupId { get; set; } = ProductIdConstants.Sportsbook;

    public bool ShowPTIcon { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;

    public string Language => UserSession.Current.LangKey;

    public int CustId { get; set; } = UserSession.Current.CustId;

    public byte RoleId { get; set; } = UserSession.Current.RoleId;

    public int ActorId => UserSession.Current.CustId;

    public byte ActorRoleId => UserSession.Current.RoleId;

    public string SiteLabel => AppSettings.SiteLabel;
}