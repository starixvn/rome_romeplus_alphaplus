using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class CheckEnableChatBotNotificationsCommand(int custId, int subAccId) : NonQueryCommand
{
    private const int ChatBotLastBetsFunctionId = 23;

    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public int FunctionId { get; } = ChatBotLastBetsFunctionId;

    public bool Status { get; set; }

    public override string GetSettingKey() => "Age_Chatbot_StatusCheck";

    public override bool IsValid() => CustId > 0 && SubAccId >= 0;
}
