using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.RegisterCashMember.Responses;

public record RegisterCashMemberResult : Result
{
    public RegisterCashMemberResult(int Code, string Message) : base(Code, Message)
    {
    }
}

public record SuggestUsernameResult : RegisterCashMemberResult
{
    public SuggestUsernameResult(int Code, string Message, string NewUsername) : base(Code, Message)
    {
        this.SeqName = NewUsername;
    }
    public string SeqName { get; init; }
}

public record ValidateCashMemberResult : RegisterCashMemberResult
{
    public ValidateCashMemberResult(int Code, string Message, string agentUsername)
        : base(Code, Message)
    {
        this.AgentUsername = agentUsername;
    }

    public string AgentUsername { get; init; }
}

public record RegisterBeforeAddNewResult : RegisterCashMemberResult
{
    public RegisterBeforeAddNewResult(int Code, string Message, int CustId) : base(Code, Message)
    {
        this.CustId = CustId;
    }

    public int CustId { get; init; }
}
