namespace AlphaPlus.Settings.Site.Features.ProductStatus.Models
{
    public class CustomerInformation
    {
        public CustomerInformation()
        {
        }

        public CustomerInformation(
            int custId,
            byte roleId,
            int superId,
            int masterId,
            int agentId)
        {
            CustId = custId;
            RoleId = roleId;
            SuperId = superId;
            MasterId = masterId;
            AgentId = agentId;
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public int SuperId { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }
    }
}