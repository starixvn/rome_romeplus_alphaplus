namespace AlphaPlus.CashService.API.Features.Transactions.Extensions;

using Shared.Models;

public static class TransactionErrorHandlers
{
    public static void OutputErrorMessageParams(ResultModel commandResult,
        decimal? agentAvailableCredit = null, decimal? memberCashBalance = null, decimal? minAmount = null, decimal? maxAmount = null)
    {
        var outputResult = new Dictionary<string, string>();
        if (agentAvailableCredit is > 0)
        {
            outputResult.Add("AgentAvailableCredit", FormatDecimal(agentAvailableCredit.Value));
        }

        if (memberCashBalance is > 0)
        {
            outputResult.Add("MemberCashBalance", FormatDecimal(memberCashBalance.Value));
        }

        if (minAmount is > 0)
        {
            outputResult.Add("MinAmount", FormatDecimal(minAmount.Value));
        }

        if (maxAmount is > 0)
        {
            outputResult.Add("MaxAmount", FormatDecimal(maxAmount.Value));
        }

        commandResult.Data = outputResult;
    }

    private static string FormatDecimal(decimal number) => number.ToString("#,##0.00");
}