using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria
{
    public class CheckUMCriteria : NonQueryCommand
    {
        public CheckUMCriteria(
            int custId,
            byte productId,
            string language)
        {
            CustId = custId;
            ProductId = productId;
            Language = language;
        }

        public int CustId { get; }

        public byte ProductId { get; }

        public byte FunctionType => 1;

        public string Language { get; }

        public bool ReturnUMData => false;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CheckUM";

        public override bool IsValid() => CustId > 0;
    }
}