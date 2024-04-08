namespace AlphaPlus.Features.Messages.API.Constants
{
    using System.Collections.Generic;

    public static class MessageType
    {
        private static readonly Dictionary<int, string> text = new Dictionary<int, string>
        {
            [Normal] = "Normal",
            [Personal] = "Personal",
            [Special] = "Special",
            [System] = "System"
        };
        public const int Normal = 0;
        public const int Personal = 1;
        public const int Special = 2;
        public const int System = 3;

        public static string ToString(int type) => text.ContainsKey(type) ? text[type] : string.Empty;
    }
}