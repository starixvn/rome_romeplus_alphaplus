namespace AlphaPlus.CashService.API.Models.PendingCommons
{
    public class BankAccountInfo
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string LoginName { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public int From { get; set; }
        
        public int To { get; set; }
        
        public string ReferenceNo { get; set; }
        
        public DateTime DateAndTime { get; set; }

        public string DateAndTimeStr => DateAndTime.ToString("MM/dd/yyyy hh:mm:ss tt");
    }
} 