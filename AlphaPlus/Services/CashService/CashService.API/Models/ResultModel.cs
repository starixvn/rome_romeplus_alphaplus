namespace AlphaPlus.CashService.API.Models
{
    public class ResultModel
    {
        public ResultModel()
        {
            Message = string.Empty;
            Code = string.Empty;
        }
        
        public ResultModel(string code, string message)
        {
            Code = code;
            Message = message;
        }
        
        public string Message { get; set; }
        public string Code { get; set; }
        public bool IsSuccessful => Code == "0";
    }
} 