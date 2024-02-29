namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models
{
    public class CustInfo
    {
        public CustInfo(int custId, string custName, byte custLevelId)
        {
            CustId = custId;
            CustName = custName;
            CustLevelId = custLevelId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }
    }
}