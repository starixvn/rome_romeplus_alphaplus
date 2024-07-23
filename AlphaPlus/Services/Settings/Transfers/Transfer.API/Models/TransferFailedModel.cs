namespace AlphaPlus.Settings.Transfer.API.Models;

public class TransferFailedModel
{
    public List<int> ListCustIds { get; set; }

    public string ErrMsg { get; set; }
}
