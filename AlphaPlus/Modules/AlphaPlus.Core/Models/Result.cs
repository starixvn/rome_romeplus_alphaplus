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

        public virtual bool IsSuccessful
        {
            get
            {
                return Code == 0;
            }
        }

        public virtual bool IsFailed
        {
            get
            {
                return !IsSuccessful;
            }
        }

        public virtual bool IsOk
        {
            get
            {
                return Code == 0;
            }
        }

        public virtual bool IsNotOk
        {
            get
            {
                return !IsOk;
            }
        }

        public static Result CreateErrorResult(string message, int? code = null)
        {
            return new Result { Code = code ?? ErrorCode, Message = message };
        }

        public static Result CreateSuccessfulResult(string message = "")
        {
            return new Result { Code = 0, Message = message };
        }
    }
}