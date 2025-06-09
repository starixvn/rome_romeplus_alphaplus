using System.Collections;
using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Core.Models;

public struct PageAccessType
{
    public const string CreateAccount = "[A]";
    public const string UpdateAccount = "[B]";
    public const string Transfer = "[C]";
    public const string ViewTotalBets = "[D]";
    public const string ViewReports = "[E]";
    public const string ViewLog = "[G]";
    public const string ViewMemberInfo = "[H]";
    public const string ViewBetLists = "[L]";
    public const string CashService = "[M]";
    public const string CashPromotion = "[N]";

    private static Hashtable accessTypes = new Hashtable
    {
        [CreateAccount] = false,
        [UpdateAccount] = false,
        [Transfer] = false,
        [ViewTotalBets] = false,
        [ViewReports] = false,
        [ViewLog] = false,
        [ViewMemberInfo] = false,
        [ViewBetLists] = false
    };

    private static Hashtable cashServiceAccessTypes = new Hashtable
    {
        [CreateAccount] = false,
        [UpdateAccount] = false,
        [Transfer] = false,
        [ViewTotalBets] = false,
        [ViewReports] = false,
        [ViewLog] = false,
        [ViewMemberInfo] = false,
        [ViewBetLists] = false,
        [CashService] = false,
        [CashPromotion] = false
    };

    public static Hashtable AccessTypes
    {
        get
        {
            return accessTypes;
        }
    }

    public static bool IsValidType(string type)
    {
        return accessTypes.ContainsKey(type);
    }
}