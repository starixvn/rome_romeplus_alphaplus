namespace AlphaPlus.Settings.Transfer.API.Models.TransferList;

public record CustomerTransferReport(
    IEnumerable<CustomerTransfer> Customers,
    CustomerTransfer Total,
    int TotalPages);
