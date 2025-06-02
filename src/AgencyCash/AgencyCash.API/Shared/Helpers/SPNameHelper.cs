namespace AgencyCash.API.Shared.Helpers;

public static class SPNameHelper
{
    public static string CustDBPrefix(string spName, short groupId)
        => groupId == 0 ? spName : $"CustDB{groupId}_{spName}";

    public static string SiteDBPrefix(string spName, short groupId)
        => groupId == 0 ? spName : $"SiteDB{groupId}_{spName}";
}