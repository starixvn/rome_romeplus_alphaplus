namespace AlphaPlus.Features.Messages.API.Constants
{
    public static class MessageType
    {
        private static readonly Dictionary<int, string> Text = new()
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

        public static string ToString(int type) => Text.TryGetValue(type, out var value) ? value : string.Empty;
    }
}