using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetEnabledProductsByGameCriteria(bool isTest) : CriteriaBase
    {
        public bool IsTest { get; } = isTest;

        public override bool IsValid() => true;

        public override string GetSettingKey() => "Acc_GameLaunchSel";
    }
}