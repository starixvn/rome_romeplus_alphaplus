namespace AlphaPlus.CashService.API.Features.Shared.Models
{
    public class WriteLogDBCommandModel
    {
        public int UserId { get; set; }
        public int CustId { get; set; }
        public int ActionId { get; set; }
        public string TrackingValue { get; set; }
        public string TrackingValue2 { get; set; }
    }
} 