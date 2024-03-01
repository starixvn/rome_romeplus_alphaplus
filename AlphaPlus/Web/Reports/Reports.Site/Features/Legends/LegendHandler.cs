using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Legends;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendHandler(ILengendService lengendService) :
    IRequestHandler<LegendQuery, LegendViewModel>,
    IRequestHandler<ProgressiveContributionLegendQuery, LegendViewModel>
{
    public async Task<LegendViewModel> Handle(LegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        if (message.IsSupportHistoricReport && !message.IsHistoricReport)
        {
            legends.Add(string.Format(
                $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
        }

        var note = await GetOfferedProductsNeedDelayTimeToSummarizeData(message.Site, message.CurrencyId, message.MinDate);
        if (!string.IsNullOrWhiteSpace(note))
        {
            legends.Add(note);
        }

        if (message.IsShowGrossComm)
        {
            legends.Add(GetGrossCommNotes(message.ViewerLevel));
        }

        return new LegendViewModel(legends, message.IsMobileFriendly);
    }

    public async Task<LegendViewModel> Handle(ProgressiveContributionLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>
        {
            Resources.ContributionAmountRoundNote
        };

        var note = await lengendService.GetOfferedProductsNeedDelayTimeToSummarizeData(
            message.Site,
            message.CurrencyId,
            message.MinDate,
            message.EnableJackpotProductIds);
        if (!string.IsNullOrWhiteSpace(note))
        {
            legends.Add(note);
        }

        return new LegendViewModel(legends);
    }

    private async Task<string> GetOfferedProductsNeedDelayTimeToSummarizeData(
        string site,
        int currencyId,
        DateTime minDate,
        IEnumerable<int> enableJackpotProductIds = null)
    {
        var productNames = await lengendService.GetOfferedProductsNeedDelayTimeToSummarizeData(site, currencyId, minDate, enableJackpotProductIds);

        return string.IsNullOrWhiteSpace(productNames)
            ? string.Empty
            : string.Format(Resources.DelayIssueLegend, productNames);
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