using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
  public class ConfirmAllowAutoMasterPTViewModel : ConfirmStatusItemViewModelBase
  {
    public ConfirmAllowAutoMasterPTViewModel(ISearchAccountInfoRequest request, int displayOrder = 6)
      : base(request, displayOrder)
    {
    }

    public override bool Visible => Actor.IsSuper && SearchAccount.IsMaster;

    public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllowedMasterAutoPT);

    public override bool ShowInCustomerList => true;

    internal override string SuffixMessage => Checked ? "InCaseDisabledMessage" : "InCaseEnabledMessage";

    internal override string StatusName => DataKey.StatusMapping.DisableAutoMasterPT;

    internal override bool StatusValue => Checked;
  }
}