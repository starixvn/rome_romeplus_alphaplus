namespace AlphaPlus.Settings.Transfer.API.Models.MultipleTransfer;

public record CustomerTransferReport(
    IEnumerable<CustomerTransfer> Customers,
    CustomerTransfer Total,
    int TotalPages);
