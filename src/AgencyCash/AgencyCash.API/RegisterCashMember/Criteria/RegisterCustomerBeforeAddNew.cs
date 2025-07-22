using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class RegisterCustomerBeforeAddNewCommand(string username, string username2, byte roleId, int uplineId) : INonQueryCommand
    {
        public string Username { get; } = username;
        public string Username2 { get; } = username2;
        public byte RoleId { get; } = roleId;
        public int UplineId { get; } = uplineId;

        // Output properties
        public int CustId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public string GetSettingKey() => "API_CustomerIns_CashMember_Register";
        public bool IsValid() => true;
    }
}
