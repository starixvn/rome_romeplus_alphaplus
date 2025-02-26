namespace AlphaPlus.Settings.Transfer.API.Models.TransferAllAndClearCredit
{
    public class TotalAmtModel
    {
        public int CustId { get; set; }

        public decimal Amount { get; set; }

        public string ToXML
            => $"<R ID=\"{CustId}\" AM=\"{Amount}\" />";
    }
}
