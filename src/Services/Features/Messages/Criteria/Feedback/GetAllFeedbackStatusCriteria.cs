using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Feedback;

public class GetAllFeedbackStatusCriteria : CriteriaBase
{
    public string SettingName { get; } = "ReviewStatus";

    public int ParentId { get; } = 0;

    public byte UserRole { get; } = 2;

    public override string GetSettingKey() => "Acc_CRM_SettingGetRelationBySettingName";

    public override bool IsValid() => true;
}
