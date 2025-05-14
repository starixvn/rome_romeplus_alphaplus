using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Shared.Request;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class AddBankAccountRequest : BaseActionRequest
{
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountNickname { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public string TransactionTypeID { get; set; }

        public string BankName { get; set; }
        public int BankId { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public DateTime LastUpdated { get; set; }

        public bool PromptPay { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public int CreatorId { get; set; }
        
        public string Bank_TransTypeList { get; set; }
        public string Agent_TransTypeList { get; set; }
        public int Agent_DepositMatchingMode { get; set; }
        public string Bank_DepositMatchingMode { get; set; }
        public DepositMatchingModeModel BankDepositMatchingMode { get; set; }
} 