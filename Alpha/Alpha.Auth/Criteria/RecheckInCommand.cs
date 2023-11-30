namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class RecheckInCommand : NonQueryCommand
    {
        public RecheckInCommand(int onlineUserId)
            => On_Userid = onlineUserId;

        public int On_Userid { get; }

        public bool IsKickedOut { get; set; }

        public byte KickOutReason { get; set; }

        public override string GetSettingKey() => "Age_ReCheckin";

        public override bool IsValid() => On_Userid > 0;
    }
}
