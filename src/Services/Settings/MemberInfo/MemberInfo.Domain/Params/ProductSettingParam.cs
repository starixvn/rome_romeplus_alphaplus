namespace AlphaPlus.Settings.MemberInfo.Domain.Params
{
    public class ProductSettingParam
    {
        public ProductSettingParam(
            int productId,
            int custId,
            string custName)
        {
            ProductId = productId;
            CustId = custId;
            CustName = custName;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public string CustName { get; }
    }
}
