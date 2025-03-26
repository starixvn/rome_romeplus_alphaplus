namespace AlphaPlus.Reports.Features.Outstanding;

public class OutstandingModel
{
    public int CustId { get; set; }

    public string CustName { get; set; }

    public decimal MemberOutstanding { get; set; }

    public decimal AgentOutstanding { get; set; }

    public decimal MasterOutstanding { get; set; }

    public decimal SuperOutstanding { get; set; }

    public decimal? ToWin { get; set; }

    public decimal? SuperRisk { get; set; }

    public decimal? MasterRisk { get; set; }

    public decimal? AgentRisk { get; set; }

    public static OutstandingModel operator +(OutstandingModel model1, OutstandingModel model2)
    {
        model1.MemberOutstanding += model2.MemberOutstanding;
        model1.AgentOutstanding += model2.AgentOutstanding;
        model1.MasterOutstanding += model2.MasterOutstanding;
        model1.SuperOutstanding += model2.SuperOutstanding;
        
        model1.ToWin = (model1.ToWin ?? 0) + (model2.ToWin ?? 0);
        model1.AgentRisk = (model1.AgentRisk ?? 0) + (model2.AgentRisk ?? 0);
        model1.MasterRisk = (model1.MasterRisk ?? 0) + (model2.MasterRisk ?? 0);
        model1.SuperRisk = (model1.SuperRisk ?? 0) + (model2.SuperRisk ?? 0);

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

    public decimal? GetSMARiskValueByLevel(byte userLevelId)
    => RoleIdConstants.IsAgentLevel(userLevelId)
        ? AgentRisk
        : RoleIdConstants.IsMasterLevel(userLevelId)
            ? MasterRisk
            : SuperRisk;
}
