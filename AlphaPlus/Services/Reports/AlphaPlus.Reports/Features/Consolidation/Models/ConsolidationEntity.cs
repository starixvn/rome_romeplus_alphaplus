using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationEntity
{
    public string BreakDown { get; init; }

    public decimal PositionTaking { get; set; }

    public string PositionTakingText => PositionTaking.FormatPTPercentage();

    public string ProductName { get; set; }

    public int CustId { get; init; }

    public byte CustLevel { get; init; }

    public string CustLevelName
    {
        get
        {
            var level = Enumeration.FromValue<UserLevel>(CustLevel);
            if (level.IsSuperLevel())
            {
                return SharedResources.Super;
            }

            if (level.IsMasterLevel())
            {
                return SharedResources.Master;
            }

            if (level.IsAgentLevel())
            {
                return SharedResources.Agent;
            }

            return SharedResources.Member;
        }
    }

    public string Username { get; init; }

    public byte CurrencyId { get; init; }

    public string CurrencyName { get; set; }

    public decimal Turnover { get; set; }

    public decimal GrossComm { get; set; }

    public decimal MemberWinLoss { get; set; }

    public decimal MemberComm { get; set; }

    public decimal MemberTotal { get; set; }

    public decimal OwnerTurnover { get; set; }

    public decimal OwnerWinLoss { get; set; }

    public decimal OwnerComm { get; set; }

    public decimal OwnerTotal { get; set; }

    public decimal Company { get; set; }

    public int BetCount { get; set; }

    public static ConsolidationEntity operator +(ConsolidationEntity model1, ConsolidationEntity model2)
    {
        model1.Turnover += model2.Turnover;
        model1.GrossComm += model2.GrossComm;
        model1.MemberWinLoss += model2.MemberWinLoss;
        model1.MemberComm += model2.MemberComm;
        model1.MemberTotal += model2.MemberTotal;
        model1.OwnerTurnover += model2.OwnerTurnover;
        model1.OwnerWinLoss += model2.OwnerWinLoss;
        model1.OwnerComm += model2.OwnerComm;
        model1.OwnerTotal += model2.OwnerTotal;
        model1.Company += model2.Company;
        model1.BetCount += model2.BetCount;

        return model1;
    }
}