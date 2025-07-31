namespace AlphaPlus.NetThrottling.Filters
{
    public interface IFilterFactory
    {
        IFilter GetFilter(string type);
    }

    public class FilterFactory(IServiceProvider serviceProvider) : IFilterFactory
    {
        private readonly Dictionary<string, IFilter> filters = [];

        private readonly IServiceProvider serviceProvider = serviceProvider;

        public IFilter GetFilter(string type)
        {
            if (filters.TryGetValue(type, out var value))
            {
                return value;
            }

            var filter = type switch
            {
                FilterTypes.IP => (IFilter)serviceProvider.GetService(typeof(IpFilter)),
                FilterTypes.Session => (IFilter)serviceProvider.GetService(typeof(SessionFilter)),
                FilterTypes.Whitelist => (IFilter)serviceProvider.GetService(typeof(WhitelistSessionFilter)),
                _ => throw new NotImplementedException()
            };

            // Use TryAdd to handle race conditions
            if (!filters.TryAdd(type, filter))
            {
                // If TryAdd fails, another thread has already added the filter
                // Get the existing filter from the dictionary
                return filters[type];
            }

            return filter;
        }
    }
}