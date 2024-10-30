namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    public partial class InsertMemberCommand
    {
        public bool IsQRCode { get; }
        public decimal CreditMax { get; private set; }
        public string Username { get; }
        public string FirstName { get; }

        public int ActorId { get; }

        public string Password { get; }

        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;

        public decimal Credit { get; }

        public int Recommend { get; }

        public string IP { get; }
        public byte SystemId { get; }
        public string Language { get; }
        public int SubAccId { get; }
        public bool IsSearchAndCopy { get; }
        public int CopyFromCustId { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public bool Success { get; set; }
        public int CustId { get; set; }
        public string CopyFromUsername { get; internal set; }
        public string SubUsername { get; internal set; }

        public string SiteDBGroupIp { get; set; }
        public string ActorUsername { get; }
    }
}
