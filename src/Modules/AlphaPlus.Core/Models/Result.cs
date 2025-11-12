namespace AlphaPlus.Core.Models
{
    public class Result
    {
        public Result()
        {
        }

        public Result(int code)
        {
            Code = code;
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public const int ErrorCode = 1;

        public int Code { get; set; }

        public string Message { get; set; }

        public virtual bool IsSuccessful => Code == 0;

        public virtual bool IsFailed => !IsSuccessful;

        public virtual bool IsOk => Code == 0;

        public virtual bool IsNotOk => !IsOk;

        public static Result CreateErrorResult(string message, int? code = null)
            => new() { Code = code ?? ErrorCode, Message = message };

        public static Result CreateSuccessfulResult(string message = "") => new() { Code = 0, Message = message };
    }
}