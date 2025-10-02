using Fanex.Data;
using Fanex.Data.Repository;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Update
{
    public class UpdateRemarkInfoCommand : NonQueryCommand, IRequest<Result>
    {
        public UpdateRemarkInfoCommand(int custId, string remark, int actorId, int subAccountId)
        {
            CustId = custId;
            Remark = remark;
            ActorId = actorId;
            SubAccountId = subAccountId;
        }
        
        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; set; }

        public string Remark { get; set; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; set; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; set; }

        public string IP { get; set; }

        public string ErrMsg { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey() => "Acc_CustomerRemarkUpd";

        public override bool IsValid() => true;
    }
}