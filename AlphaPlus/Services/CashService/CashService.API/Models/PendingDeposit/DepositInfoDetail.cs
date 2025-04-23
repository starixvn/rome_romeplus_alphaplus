namespace AlphaPlus.CashService.API.Models.PendingDeposit
{
    public class DepositInfoDetail
    {
        public string MemberLoginName { get; set; }
        
        public int From { get; set; }
        
        public int To { get; set; }
        
        public decimal DepositAmount { get; set; }
        
        public string Remarks { get; set; }
        
        public string ImagePath { get; set; }
    }
} 