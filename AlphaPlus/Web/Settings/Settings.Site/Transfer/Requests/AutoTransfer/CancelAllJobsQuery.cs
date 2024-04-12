namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using Microsoft.AspNetCore.Mvc;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class CancelAllJobsQuery : IRequest<Result>
    {
        public CancelAllJobsQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        
        public string Username { get; set; }

        public string Status { get; set; }

        public bool IsValid() => UplineId > 0;
    }
}