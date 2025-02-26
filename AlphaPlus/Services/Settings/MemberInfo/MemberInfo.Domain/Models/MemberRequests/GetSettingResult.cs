namespace AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests
{
    public class GetSettingResult
    {
        public GetSettingResult(
            bool hasSetting,
            int errorCode,
            string errorMessage)
        {
            HasSetting = hasSetting;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public bool HasSetting { get; protected set; }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }
    }
}
