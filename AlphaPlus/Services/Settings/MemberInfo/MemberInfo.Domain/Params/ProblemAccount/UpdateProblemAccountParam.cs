namespace AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount
{
    public class UpdateProblemAccountParam
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string CustomerList { get; set; }

        public byte RoleId { get; set; }

        public bool IsAffectedAllDownlines { get; set; }

        public string Language { get; set; }

        public string LoginIp { get; set; }
    }
}
