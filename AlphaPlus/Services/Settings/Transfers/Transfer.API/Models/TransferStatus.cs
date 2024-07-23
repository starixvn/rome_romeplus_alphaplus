namespace AlphaPlus.Settings.Transfer.API.Models;

public class TransferStatus
{
    public List<int> listCustIdsTransferSuccess { get; set; }
    public List<TransferCacheModel> listCustIdsTransferConcurrent { get; set; }
    public List<TransferFailedModel> listCustIdsTransferError { get; set; }

    public TransferStatus()
    {
        listCustIdsTransferSuccess = new List<int>();
        listCustIdsTransferConcurrent = new List<TransferCacheModel>();
        listCustIdsTransferError = new List<TransferFailedModel>();
    }
}
