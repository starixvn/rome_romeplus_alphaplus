namespace AlphaPlus.CashService.API.Features.Shared.Models
{
    public class ResultModel(int code, string message)
    {
        public ResultModel() : this(0, string.Empty)
        {
        }

        public int Code { get; set; } = code;
        public string Message { get; set; } = message;
        public bool IsSuccessful => Code == 0;
    }
}