namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    public partial class InsertMemberCommand
    {
        private readonly byte alphaSystemId = 2;

        public InsertMemberCommand(
            string username,
            string firstName,
            int actorId,
            int custIdCopyFrom,
            string ip,
            string language,
            int subAccId,
            decimal credit,
            string password,
            string copyFromUsername,
            decimal creditMax,
            string subUsername,
            string siteDBGroupIp,
            string actorUsername)
        {
            SubUsername = subUsername;
            IsQRCode = true;
            Credit = credit;
            CreditMax = creditMax;
            CopyFromUsername = copyFromUsername;
            Username = username;
            FirstName = firstName;
            ActorId = actorId;
            Recommend = actorId;
            CopyFromCustId = custIdCopyFrom;
            Password = password;
            IP = ip;
            Language = language;
            SubAccId = subAccId;
            IsSearchAndCopy = true;
            SystemId = alphaSystemId;
            SiteDBGroupIp = siteDBGroupIp;
            ActorUsername = actorUsername;
        }
    }
}
