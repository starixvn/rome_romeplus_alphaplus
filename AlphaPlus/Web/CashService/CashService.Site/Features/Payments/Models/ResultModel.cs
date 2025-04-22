namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class ResultModel
    {
        public ResultModel()
        {
            Code = 0;
            Message = string.Empty;
        }
        
        public ResultModel(int code, string message)
        {
            Message = message;
            Code = code;
        }
        
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful => Code == 0;
    }
} 