namespace AlphaPlus.NetThrottling.Resolvers
{
    public interface IResolverFactory
    {
        IResolver GetResolver(string type);
    }

    public class ResolverFactory : IResolverFactory
    {
        private readonly Dictionary<string, IResolver> resolvers = [];

        public IResolver GetResolver(string type)
        {
            if (resolvers.TryGetValue(type, out IResolver value))
            {
                return value;
            }

            var resolver = type switch
            {
                ResolverTypes.HttpError => new HttpErrorResolver(),
                ResolverTypes.Base => new BaseResolver(),
                _ => throw new NotImplementedException()
            };

            resolvers.Add(type, resolver);

            return resolver;

        }
    }
}