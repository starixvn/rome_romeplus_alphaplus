using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests;

public class TransferBaseQuery
{
    protected readonly IUser viewer;

    public TransferBaseQuery() : this(null)
    {
    }

    protected TransferBaseQuery(IUser viewer)
    {
        this.viewer = viewer ?? new User(new UserSessionExt());
        // Default data, if query string had data, then get from query string.
        CustId = this.viewer.Id;
        CustName = this.viewer.Name;
        CustLevelId = this.viewer.LevelId;
        CustLevel = Enumeration.FromValue<UserLevel>(CustLevelId);

        ViewerId = this.viewer.Id;
        ViewerLevelId = this.viewer.LevelId;
        ActorLevel = Enumeration.FromValue<UserLevel>(ViewerLevelId);
        AGIP = this.viewer.AGIP;
    }

    public int CustId { get; set; }

    public string CustName { get; set; }

    public byte CustLevelId { get; set; }

    public virtual UserLevel CustLevel { get; }

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public virtual UserLevel ActorLevel { get; }

    public string AGIP { get; }

    public virtual bool IsValid() => CustId > 0 && ActorLevel.IsEqualOrUplineOf(CustLevel);
}
