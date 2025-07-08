namespace AlphaPlus.Main.Site.Features.ProtectionCode
{
    public class ProtectionCodeViewModel
    {
        public ProtectionCodeViewModel(
            int custId,
            string userLevel,
            string language,
            string hashKey,
            bool isExistedProtectionCode,
            bool enableSecurityCode)
        {
            CustId = custId;
            UserLevel = userLevel;
            Language = language;
            HashKey = hashKey;
            IsExistedProtectionCode = isExistedProtectionCode;
            EnableSecurityCode = enableSecurityCode;
        }

        public int CustId { get; }

        public string UserLevel { get; }

        public string Language { get; }

        public string HashKey { get; }

        public bool IsExistedProtectionCode { get; }

        public bool EnableSecurityCode { get; }

        public string GetLearnMoreViewLang()
            => new[] { "th-TH", "vi-VN", "zh-CN", "zh-TW", "ko-KR" }.FirstOrDefault(l => l.Equals(Language)) ?? "en-US";
    }
}