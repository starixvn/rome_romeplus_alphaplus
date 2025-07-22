using System.Text.RegularExpressions;

namespace AgencyCash.API.Shared.Helpers;

public static class EndpointHelper
{
    public const string RegisterCashMember = "Register Cash Member";
    private static readonly Dictionary<string, string> FunctionNameMapper = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/api/cash-members/check-username"] = "Check Cash Member Username",
        ["/api/cash-members"] = "Register Cash Member",
        ["/api/deposit/limit"] = "Min Max Deposit",
        ["/api/deposit/manual"] = "Manual Deposit",
        ["/api/deposit/auto"] = "Auto Deposit",
        ["/api/withdrawal/limit"] = "Min Max Withdrawal",
        ["/api/withdrawal/manual"] = "Manual Withdrawal",
    };

    public static string ToFunctionName(string path)
    {
        path = path.TrimEnd('/');

        if (FunctionNameMapper.TryGetValue(path, out var functionName))
        {
            return functionName;
        }

        // Handle dynamic segments like {transId}
        return MapDynamicPath(path);
    }

    private static string MapDynamicPath(string path)
    {
        if (Regex.IsMatch(path, @"/api/deposit/[^/]+/status$", RegexOptions.IgnoreCase))
        {
            return "Update Deposit Transaction Status";
        }

        if (Regex.IsMatch(path, @"/api/deposit/[^/]+/payslip$", RegexOptions.IgnoreCase))
        {
            return "Add Payslip To Manual Deposit";
        }

        if (Regex.IsMatch(path, @"/api/withdrawal/[^/]+/status$", RegexOptions.IgnoreCase))
        {
            return "Update Withdrawal Transaction Status";
        }

        return string.Empty;
    }

    public static bool IsRegisterCashMember(string functionName)
        => functionName.Equals(RegisterCashMember, StringComparison.OrdinalIgnoreCase);
}