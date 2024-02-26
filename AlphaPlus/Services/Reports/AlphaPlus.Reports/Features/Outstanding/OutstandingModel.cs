namespace AlphaPlus.Reports.Features.Outstanding;

public class OutstandingModel
{
    public OutstandingModel()
    {
    }

    public OutstandingModel(int custId, string custName, decimal memberOutstanding, decimal agentOutstanding, decimal masterOutstanding, decimal superOutstanding)
    {
        CustId = custId;
        CustName = custName;
        MemberOutstanding = memberOutstanding;
        AgentOutstanding = agentOutstanding;
        MasterOutstanding = masterOutstanding;
        SuperOutstanding = superOutstanding;
    }

    public string DownlineLink { get; init; }

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

    public static OutstandingModel GetAccumulatedData(IEnumerable<OutstandingModel> outstandings)
    {
        var accumulatedData = new OutstandingModel();

        foreach (var outstanding in outstandings)
        {
            accumulatedData.MemberOutstanding += outstanding.MemberOutstanding;
            accumulatedData.AgentOutstanding += outstanding.AgentOutstanding;
            accumulatedData.MasterOutstanding += outstanding.MasterOutstanding;
            accumulatedData.SuperOutstanding += outstanding.SuperOutstanding;
        }

        return accumulatedData;
    }

    public decimal GetValueByLevel(UserLevel level)
        => level.IsMemberLevel()
            ? MemberOutstanding
            : level.IsAgentLevel()
                ? AgentOutstanding
                : level.IsMasterLevel()
                    ? MasterOutstanding
                    : SuperOutstanding;
}
