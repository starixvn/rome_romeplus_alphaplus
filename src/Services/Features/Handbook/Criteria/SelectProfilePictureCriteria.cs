using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Features.Handbook.API.Criteria
{
    public class SelectProfilePictureCriteria : CriteriaBase
    {
        public SelectProfilePictureCriteria(int actorId)
        {
            ActorId = actorId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; private set; }

        public override string GetSettingKey() => "Age_CustomerSelProfilePicture";

        public override bool IsValid() => ActorId > 0;
    }
}
