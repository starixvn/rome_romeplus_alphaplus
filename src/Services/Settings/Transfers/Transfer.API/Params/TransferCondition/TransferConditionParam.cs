namespace AlphaPlus.Settings.Transfer.API.Params.TransferCondition;

public record TransferConditionParam(
    int ActorId,
    byte ActorLevelId,
    int CustId,
    string TransferOption,
    bool TurnOffAutoTransfer,
    int SubAccId,
    string IP,
    string Language,
    string AGIP);
