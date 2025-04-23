namespace AlphaPlus.Main.Site.Features.UMMessage
{
    public class UMMessageItem
    {
        public UMMessageItem()
        {
        }

        public UMMessageItem(
            int productId,
            int sportType,
            byte settingType,
            byte functionType,
            DateTime beginTime,
            DateTime endTime,
            byte[] time_Stamp)
        {
            ProductId = productId;
            SportType = sportType;
            SettingType = settingType;
            FunctionType = functionType;
            BeginTime = beginTime;
            EndTime = endTime;
            Time_Stamp = time_Stamp;
        }

        public int ProductId { get; init; }

        public int SportType { get; init; }

        public byte SettingType { get; init; }

        public byte FunctionType { get; init; }

        public DateTime BeginTime { get; init; }

        public DateTime EndTime { get; init; }

        public byte[] Time_Stamp { get; init; }
    }
}
