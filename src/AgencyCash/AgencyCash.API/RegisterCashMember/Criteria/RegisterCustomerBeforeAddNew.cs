using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class RegisterCustomerBeforeAddNewCommand : INonQueryCommand
    {
        public RegisterCustomerBeforeAddNewCommand(string username, string username2, byte roleId, int uplineId)
        {
            Username = username;
            Username2 = username2;
            RoleId = roleId;
            UplineId = uplineId;
        }

        public string Username { get; }
        public string Username2 { get; }
        public byte RoleId { get; }
        public int UplineId { get; }

        // Output properties
        public int CustId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public string GetSettingKey() => "API_CustomerIns_CashMember_Register";
        public bool IsValid() => true;
    }
}
