namespace AlphaPlus.CashService.Site.Features.Promotion.Enum;

using System.ComponentModel;
using AlphaPlus.Shared.AppResources.CashServices;

public enum PromotionType
{
    [Description(nameof(CashServicesResx.PromotionType_RebateOnTurnover))]
    RebateOnTurnover = 1,
}