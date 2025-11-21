using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.Helpers
{
    public static class JsonHelper
    {
        [ThreadStatic]
        private static Func<object, string> toJsonFunc;

        public static Func<object, string> ToJsonFunc
        {
            get
            {
                return toJsonFunc ?? ToJson;
            }

            set
            {
                toJsonFunc = value;
            }
        }

        public static string ToJson(object data) => JsonConvert.SerializeObject(data);
    }
}
