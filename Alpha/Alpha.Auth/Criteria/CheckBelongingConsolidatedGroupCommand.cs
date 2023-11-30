namespace Alpha.Auth.Criteria
{
    internal class CheckBelongingConsolidatedGroupCommand : CheckBelongingCommand
    {
        public CheckBelongingConsolidatedGroupCommand(int sessionCustId, int? custId, int subAccId, long transId)
            : base(sessionCustId, custId, subAccId, transId)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_CustomerConsolidatedGroup_CheckBelonging";
    }
}
