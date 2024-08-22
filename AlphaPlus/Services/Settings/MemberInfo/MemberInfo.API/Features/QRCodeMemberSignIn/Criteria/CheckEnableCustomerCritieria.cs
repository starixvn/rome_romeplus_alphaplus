namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    using Fanex.Data.Repository;

    public class CheckEnableCustomerCritieria : NonQueryCommand
    {
        public CheckEnableCustomerCritieria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CustomerSelCheckEnable";

        public override bool IsValid() => CustId > 0;
    }
}
