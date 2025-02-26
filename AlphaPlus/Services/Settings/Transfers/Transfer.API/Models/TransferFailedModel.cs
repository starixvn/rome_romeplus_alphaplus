namespace AlphaPlus.Settings.Transfer.API.Models;

public record TransferFailedModel(List<int> CustIds, string ErrMsg);
