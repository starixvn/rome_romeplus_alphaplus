// using AgencyCash.API.Shared.Helpers;
// using Fanex.Data.Repository;

// namespace AgencyCash.API.Payments.Criteria;

// public class GetAgentServiceTypeCriteria(int custId, short custDBGroupId) : NonQueryCommand
// {
//     public int CustId { get; } = custId;

//     public byte ServiceType { get; set; }

//     public override string GetSettingKey()
//         => SPNameHelper.CustDBPrefix("Age_CashService_CustServiceTypeSel", custDBGroupId);

//     public override bool IsValid() => CustId > 0;
// }