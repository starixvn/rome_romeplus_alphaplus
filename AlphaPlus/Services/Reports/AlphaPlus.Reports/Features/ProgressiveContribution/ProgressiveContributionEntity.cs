namespace AlphaPlus.Reports.Features.ProgressiveContribution;

public class ProgressiveContributionEntity
{
    public int ProductId { get; set; }

    public int BetType { get; set; }

    public string BetTypeName { get; set; }

    public decimal Turnover { get; set; }

    public decimal Contribution { get; set; }

    public decimal ContributionAgent { get; set; }

    public decimal ContributionMaster { get; set; }

    public decimal ContributionSuper { get; set; }

    public decimal CompanyContribution
    {
        get => -(ContributionSuper + ContributionMaster + ContributionAgent);
        set => _ = value;
    }

    public static ProgressiveContributionEntity operator +(ProgressiveContributionEntity model1, ProgressiveContributionEntity model2)
    {
        model1.Turnover += model2.Turnover;
        model1.Contribution += model2.Contribution;
        model1.ContributionAgent += model2.ContributionAgent;
        model1.ContributionMaster += model2.ContributionMaster;
        model1.ContributionSuper += model2.ContributionSuper;
        model1.CompanyContribution += model2.CompanyContribution;

        return model1;
    }
}