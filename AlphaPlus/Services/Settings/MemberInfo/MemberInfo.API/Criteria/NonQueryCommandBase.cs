namespace AlphaPlus.Settings.MemberInfo.API.Criteria
{
    using Fanex.Data.Repository;

    public class NonQueryCommandBase : NonQueryCommand
    {
        protected NonQueryCommandBase(int actorId, string ip, string language)
        {
            UserId = actorId;
            IP = ip;
            Language = language;
        }

        public int UserId { get; }

        public string IP { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid() => UserId > 0;
    }
}
