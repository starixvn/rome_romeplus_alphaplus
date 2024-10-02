using Fanex.Customer.Client.Models;

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

        public int MsgType { get; set; }

        public MessageContent MessageContent { get; set; }
    }
}