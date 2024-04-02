using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Legends;
using MediatR;
using AlphaPlus.Reports.Features.Shared.Models;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendHandler(ILengendService lengendService, IOptions<AppSettings> options) :
    IRequestHandler<LegendQuery, LegendViewModel>,
    IRequestHandler<WinLossLegendQuery, LegendViewModel>,
    IRequestHandler<WinLossDetailLegendQuery, LegendViewModel>,
    IRequestHandler<MatchWinLossDetailLegendQuery, LegendViewModel>
{
    public async Task<LegendViewModel> Handle(LegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, message);

        return new LegendViewModel(legends);
    }

    public async Task<LegendViewModel> Handle(WinLossLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        if (!message.IsHistoricReport && message.MinDate != DateTime.MinValue)
        {
            legends.Add(string.Format(
                $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
        }

        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, message);
        legends.Add(GetGrossCommNotes(message.ViewerLevel));
        legends.Add(Resources.GrossCommRoundingNote);
        if (message.ShowDormantAccountsNote)
        {
            legends.Add(Resources.DormantAccountsNote);
        }

        return new LegendViewModel(legends);
    }

    public async Task<LegendViewModel> Handle(WinLossDetailLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>
        {
            Resources.Tooltip_NetTurnover
        };

        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            options.Value.SettingStatus,
            message.Language,
            message.MinDate);
        var productNames = lengendService.GetProductNamesForLiveCasinoTooltip(productParam);
        if (!string.IsNullOrWhiteSpace(productNames))
        {
            legends.Add(string.Format(Resources.Tooltip_NetTurnover_livecasino, productNames));
        }

        productNames = lengendService.GetProductNamesForRNGCasinoTooltip(productParam);
        if (!string.IsNullOrWhiteSpace(productNames))
        {
            legends.Add(string.Format(Resources.Tooltip_NetTurnover_RNGCasino, productNames));
        }

        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, message);
        legends.Add(GetGrossCommNotes(message.ViewerLevel));
        legends.Add(Resources.GrossCommRoundingNote);
        // if (message.ShowDormantAccountsNote)
        // {
        //     legends.Add(Resources.DormantAccountsNote);
        // }

        return new LegendViewModel(legends);
    }

    public Task<LegendViewModel> Handle(MatchWinLossDetailLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>(GetGrossCommNotesByUserLevel(message.ViewerLevel));
        var viewModel = new LegendViewModel([$"- {Resources.GrossComm}: <ul class=\"gross-comm\">{string.Join(string.Empty, legends)}</ul>"]);

        return Task.FromResult(viewModel);
    }

    private async Task AddOfferedProductsNeedDelayTimeToSummarizeData(
        IList<string> legends,
        LegendQuery message,
        IEnumerable<int> enableJackpotProductIds = null)
    {
        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            options.Value.SettingStatus,
            message.Language,
            message.MinDate);
        var productNames = await lengendService.GetOfferedProductsNeedDelayTimeToSummarizeData(productParam, enableJackpotProductIds);

        if (!string.IsNullOrWhiteSpace(productNames))
        {
            legends.Add(string.Format(Resources.DelayIssueLegend, productNames));
        }
    }

    private static string GetGrossCommNotes(UserLevel viewerLevel)
    {
        var grossCommNotes = new List<string>(GetGrossCommNotesByUserLevel(viewerLevel))
        {
            $"<li>{Resources.GrossCommNoteForCasino}</li>"
        };

        if (viewerLevel.IsNotAgentLevel())
        {
            grossCommNotes.Add($"<li>{Resources.GrossCommNoteForRoundingProblem}</li>");
        }

        return $"- {Resources.GrossComm}: <ul class=\"gross-comm\">{string.Join(string.Empty, grossCommNotes)}</ul>";
    }

    private static IEnumerable<string> GetGrossCommNotesByUserLevel(UserLevel viewerLevel)
    {
        if (viewerLevel.IsSuperLevel())
        {
            return [
                $"<li>{Resources.GrossCommNoteForMaster}</li>",
                $"<li>{Resources.GrossCommNoteForAgent}</li>",
                $"<li>{Resources.GrossCommNoteForMember}</li>"
            ];
        }

        if (viewerLevel.IsMasterLevel())
        {
            return [
                $"<li>{Resources.GrossCommNoteForAgent}</li>",
                $"<li>{Resources.GrossCommNoteForMember}</li>"
            ];
        }

        return [$"<li>{Resources.GrossCommNoteForMember}</li>"];
    }
}