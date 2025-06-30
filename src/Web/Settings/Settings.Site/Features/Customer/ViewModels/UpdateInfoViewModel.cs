namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;
public class UpdateInfoViewModel
{
    public UpdateInfoViewModel(
         string assetsVersion,
         int custId,
         string username,
         bool isCashMember)
    {
        AssetsVersion = assetsVersion;
        CustId = custId;
        Username = username;
        IsCashMember = isCashMember;
    }

    public string AssetsVersion { get; }

    public bool IsCashMember { get; }

    public int CustId { get; }

    public string Username { get; }
}