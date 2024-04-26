namespace AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models
{
    public class ResultCreateMemberQRCode
    {
        public int CustId { get; set; }

        public int CustIdCopyFrom { get; set; }

        public string? RawPassword { get; set; }

        public bool Success { get; set; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }
    }
}