using System;

namespace AlphaPlus.CashService.API.Models.PendingDeposit
{
    public class AccountInfoDetail
    {
        public string MemberLoginName { get; set; }
        
        public string From { get; set; }
        
        public string To { get; set; }
        
        public decimal DepositAmount { get; set; }
        
        public string Remarks { get; set; }
        
        public string ImagePath { get; set; }
    }
} 