namespace Alpha.Main.Site.Features.Nickname
{

    public class UpdateSubAccountNicknameLoginOnlyCommand(UpdateNicknameLoginOnlyParam param) : UpdateNicknameLoginOnlyCommand(param)
    {
        public int SubAccId { get; } = param.SubAccountId;

        public override string GetSettingKey()
            => "Age_CustSubUpdNicknameLoginOnly";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
