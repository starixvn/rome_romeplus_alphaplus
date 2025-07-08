
namespace AgencyCash.API.Shared.Helpers;

public static class EndpointHelper
{
    public const string RegisterCashMember = "RegisterCashMember";

    public static string ToFunctionName(string path)
    {
        if (path.Contains("cash-members", StringComparison.OrdinalIgnoreCase))
        {
            return RegisterCashMember;
        }

        if (path.Contains("deposit", StringComparison.OrdinalIgnoreCase))
        {
            return "Deposit";
        }

        if (path.Contains("withdrawal", StringComparison.OrdinalIgnoreCase))
        {
            return "Withdrawal";
        }

        return string.Empty;
    }

    public static bool IsRegisterCashMember(string functionName)
        => functionName.Equals(RegisterCashMember, StringComparison.OrdinalIgnoreCase);
}