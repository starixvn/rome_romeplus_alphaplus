using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer;

public class AutoTransferViewModel
{
    protected AutoTransferViewModel(IUserSession userSession)
    {
        ViewerLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
        ShowLevelType = ViewerLevel.IsMasterOrSuperLevel();
        ViewerId = userSession.CustId;
        SubAccId = userSession.SubAccId;
    }

    public int ViewerId { get; }

    public int SubAccId { get; }

    public UserLevel ViewerLevel { get; }

    public bool ShowLevelType { get; }

    public IEnumerable<SelectListItem> Columns { get; protected set; }

    public string UserGuideLink
    {
        get
        {
            if (AppSettings.SupportedNewHandbook)
            {
                return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=auto-transfer");
            }

            return AppSettings.IsSiteLabel789y
                ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=184099")
                : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=184099");
        }
    }
}