namespace AgencyCash.API.Shared.Helpers
{
    public static class DomainHelper
    {
        private const int HttpProtocolLength = 7;
        private const int HttpsProtocolLength = 8;

        public static string RemoveProtocol(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                return domain;
            }

            if (domain.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return domain[HttpProtocolLength..];
            }

            if (domain.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return domain[HttpsProtocolLength..];
            }

            return domain;
        }
    }
}
