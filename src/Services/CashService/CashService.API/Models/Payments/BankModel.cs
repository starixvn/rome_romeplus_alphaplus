using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankModel
    {
        public string TransactionTypeID { get; set; }
        public string TransTypeList { get; set; }

        public string Bank { get; set; }
        public string DepositMode { get; set; }
        public string Code { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TransactionTypes => TransactionTypeID ?? TransTypeList;
        public DepositMatchingModeModel DepositMatchingMode 
        {
            get
            {
                return string.IsNullOrEmpty(DepositMode) ? null : JsonConvert.DeserializeObject<DepositMatchingModeModel>(DepositMode);
            }
        }
    }
}