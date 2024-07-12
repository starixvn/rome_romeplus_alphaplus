namespace AlphaPlus.Settings.Site.Features.Transfer.Requests
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using Settings.Transfer.API.Models;

    public class TransferBaseQuery
    {
        protected readonly IUser viewer;

        public TransferBaseQuery() : this(null) { }

        protected TransferBaseQuery(IUser viewer)
        {
            this.viewer = viewer ?? new User(UserSession.Current);

            // Default data, if query string had data, then get from query string.
            CustId = this.viewer.Id;
            CustName = this.viewer.Name;
            CustLevelId = this.viewer.LevelId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => viewer.Id;

        public byte ViewerLevelId => viewer.LevelId;

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public string Language => viewer.Language;

        public string AGIP => viewer.AGIP;

        public virtual bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(CustName)
            && ActorLevel.IsEqualOrUplineOf(CustLevel)
            && !string.IsNullOrWhiteSpace(Language);
    }
}