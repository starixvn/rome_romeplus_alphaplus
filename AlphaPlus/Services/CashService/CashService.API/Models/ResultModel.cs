namespace AlphaPlus.CashService.API.Models
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
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful => Code == 0;
    }
}