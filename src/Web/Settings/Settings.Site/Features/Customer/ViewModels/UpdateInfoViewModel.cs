namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;
public class UpdateInfoViewModel
{
    public UpdateInfoViewModel(
         string assetsVersion,
         int custId,
         string username,
         bool isCashMember,
         string cdnUri)
    {
        AssetsVersion = assetsVersion;
        CustId = custId;
        Username = username;
        IsCashMember = isCashMember;
        CdnUri = cdnUri;
    }

    public string AssetsVersion { get; }

    public bool IsCashMember { get; }

    public int CustId { get; }

    public string Username { get; }

    public string CdnUri { get; }
}