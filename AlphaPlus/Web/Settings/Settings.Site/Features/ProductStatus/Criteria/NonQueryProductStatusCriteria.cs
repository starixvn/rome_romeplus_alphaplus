using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public abstract class NonQueryProductStatusCriteria : NonQueryCommand
    {
        private const byte AlphaSystemId = 2;

        protected NonQueryProductStatusCriteria(
            int custId,
            int subAccId,
            int actorId,
            string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            ActorId = actorId;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        public string IP { get; }

        public byte SystemId { get; } = AlphaSystemId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public virtual int MsgType { get; set; }

        [SpParam(Ignored = true)]
        public bool Success => ErrCode.Equals(0);

        public override bool IsValid()
            => CustId > 0
            && SubAccId >= 0
            && ActorId > 0;
    }
}