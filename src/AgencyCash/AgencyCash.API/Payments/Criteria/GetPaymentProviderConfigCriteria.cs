using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class GetPaymentProviderConfigCriteria(PaymentProvider provider, int siteDBGroupId) : CriteriaBase
{
    public PaymentProvider Provider { get; } = provider;

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashService_PaymentProviderConfigSel", siteDBGroupId);

    public override bool IsValid() => true;
} 