namespace AlphaPlus.Reports.Features.ProgressiveContribution;

public class ProgressiveContributionEntity
{
    public int ProductId { get; set; }

    public int BetTypeId { get; set; }

    public string BetTypeName { get; set; }

    public decimal Turnover { get; set; }

    public decimal ContributionAmount { get; set; }

    public decimal AgentContribution { get; set; }

    public decimal MasterContribution { get; set; }

    public decimal SuperContribution { get; set; }

    public decimal CompanyContribution
    {
        get => -(SuperContribution + MasterContribution + AgentContribution);
        set => _ = value;
    }

    public static ProgressiveContributionEntity operator +(ProgressiveContributionEntity model1, ProgressiveContributionEntity model2)
    {
        model1.Turnover += model2.Turnover;
        model1.ContributionAmount += model2.ContributionAmount;
        model1.AgentContribution += model2.AgentContribution;
        model1.MasterContribution += model2.MasterContribution;
        model1.SuperContribution += model2.SuperContribution;
        model1.CompanyContribution += model2.CompanyContribution;

        return model1;
    }
}