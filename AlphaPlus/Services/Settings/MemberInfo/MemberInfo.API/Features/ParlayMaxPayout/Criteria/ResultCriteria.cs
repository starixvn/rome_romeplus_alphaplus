namespace AlphaPlus.Settings.MemberInfo.API.Features.ParlayMaxPayout.Criteria
{
    using Fanex.Data.Repository;

    public class ResultCriteria : NonQueryCommand
    {
        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        public override string GetSettingKey() => string.Empty;

        public override bool IsValid() => true;
    }
}