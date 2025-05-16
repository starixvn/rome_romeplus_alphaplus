namespace AgencyCash.API.Shared.Helpers;

public static class SPNameHelper
{
    public static string CustDBPrefix(string spName, int? groupId)
        => !groupId.HasValue ? spName : $"CustDB{groupId.Value}_{spName}";

    public static string CustDBPrefix(string spName, int groupId)
        => groupId == 0 ? spName : $"CustDB{groupId}_{spName}";

    public static string SiteDBPrefix(string spName, int groupId)
        => groupId == 0 ? spName : $"SiteDB{groupId}_{spName}";
}