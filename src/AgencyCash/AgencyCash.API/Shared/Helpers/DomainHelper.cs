using System;

namespace AgencyCash.API.Shared.Helpers
{
    public static class DomainHelper
    {
        public static string RemoveProtocol(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return domain;

            if (domain.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return domain.Substring(7);
            if (domain.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return domain.Substring(8);

            return domain;
        }
    }
}
