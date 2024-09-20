namespace AlphaPlus.NetThrottling.Utils
{
    public static class TypeFactory<T>
    {
        private static readonly IDictionary<string, T> cache = new Dictionary<string, T>();

        public static T Create(string typeName)
        {
            if (!cache.ContainsKey(typeName))
            {
                lock (cache)
                {
                    if (!cache.ContainsKey(typeName))
                    {
                        var filter = InternalCreate(typeName);
                        cache.Add(typeName, filter);
                        return filter;
                    }
                }
            }

            return cache[typeName];
        }

        private static T InternalCreate(string typeName)
        {
            var type = Type.GetType(typeName) ?? throw new InvalidCastException($"{typeName} is invalid");
            var instance = Activator.CreateInstance(type);
            if (instance is not T filter)
            {
                throw new InvalidCastException($"{typeName} is not a type of {typeof(T)}");
            }

            return filter;
        }
    }
}
