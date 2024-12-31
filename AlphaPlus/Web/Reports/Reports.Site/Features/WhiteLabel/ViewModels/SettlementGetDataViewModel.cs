using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public record SettlementGetDataViewModel(UserLevel ViewerLevel, ReportData<SettlementModel> ReportData);