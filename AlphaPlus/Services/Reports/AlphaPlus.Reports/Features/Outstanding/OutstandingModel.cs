namespace AlphaPlus.Reports.Features.Outstanding;

public class OutstandingModel
{
    public int CustId { get; set; }

    public string CustName { get; set; }

    public decimal MemberOutstanding { get; set; }

    public decimal AgentOutstanding { get; set; }

    public decimal MasterOutstanding { get; set; }

    public decimal SuperOutstanding { get; set; }

    public static OutstandingModel operator +(OutstandingModel model1, OutstandingModel model2)
    {
        model1.MemberOutstanding += model2.MemberOutstanding;
        model1.AgentOutstanding += model2.AgentOutstanding;
        model1.MasterOutstanding += model2.MasterOutstanding;
        model1.SuperOutstanding += model2.SuperOutstanding;

        return model1;
    }

    public decimal GetValueByLevel(byte userLevelId)
    => RoleIdConstants.IsMemberLevel(userLevelId)
        ? MemberOutstanding
        : RoleIdConstants.IsAgentLevel(userLevelId)
            ? AgentOutstanding
            : RoleIdConstants.IsMasterLevel(userLevelId)
                ? MasterOutstanding
                : SuperOutstanding;
}
