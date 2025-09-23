using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Get
{
    public class GetRemarkInfoCriteria : CriteriaBase
    {
        public GetRemarkInfoCriteria(int searchAccountId, int actorId)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; set; }


        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; set; }

        public override string GetSettingKey() => "Acc_CustomerRemarkSel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}