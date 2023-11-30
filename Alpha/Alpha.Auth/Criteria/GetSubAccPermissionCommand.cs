namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class GetSubAccPermissionCommand : NonQueryCommand
    {
        public GetSubAccPermissionCommand(int subAccId)
            => SubAccId = subAccId;

        public int SubAccId { get; }

        public string PageAccess { get; set; }

        public override string GetSettingKey() => "Age_CustSubPageAccessSel";

        public override bool IsValid() => SubAccId >= 0;
    }
}
