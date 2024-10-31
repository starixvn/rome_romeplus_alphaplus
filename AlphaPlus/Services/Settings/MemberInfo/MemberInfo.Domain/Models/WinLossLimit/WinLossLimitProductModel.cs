namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    public class WinLossLimitProductModel
    {
        public WinLossLimitProductModel()
        {
        }

        public WinLossLimitProductModel(
            int betType,
            int sportType,
            string productName,
            int productId,
            int type,
            int orderNo,
            string playerId = null)
        {
            BetType = betType;
            SportType = sportType;
            ProductName = productName;
            ProductId = productId;
            PlayerId = playerId ?? "-";
            Type = type;
            OrderNo = orderNo;
        }

        public int ProductId { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string PlayerId { get; set; } = string.Empty;

        public int Type { get; set; }

        public string TypeName { get; set; } = string.Empty;

        public int OrderNo { get; set; }
    }
}
