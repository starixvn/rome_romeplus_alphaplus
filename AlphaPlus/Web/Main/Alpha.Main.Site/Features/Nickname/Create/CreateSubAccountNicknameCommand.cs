namespace Alpha.Main.Site.Features.Nickname
{

    public class CreateSubAccountNicknameCommand(CreateNicknameParam param) : CreateNicknameNonQueryCommand(param)
    {
        public int SubAccId { get; } = param.SubAccountId;

        public override string GetSettingKey()
            => "Age_CustSubInsNickname";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
