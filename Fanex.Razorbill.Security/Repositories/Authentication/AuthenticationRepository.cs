namespace Fanex.Razorbill.Security.Repositories.Authentication
{
    using System;
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Razorbill.Security.Entities;

    public class AuthenticationRepository : IAuthenticationRepository
    {
        public ClosedLockedStatus CheckClosedOrLockedStatus(int customerId, int subAccountId)
        {
            using (var db = new ObjectDb("Age_CustomerStatusSel"))
            {
                db.AssignParamValues(new Hashtable
                {
                    ["custid"] = customerId,
                    ["subaccid"] = subAccountId,
                    ["isactive"] = false,
                    ["locked"] = 0,
                });

                db.ExecuteNonQuery();
                var ouputParams = db.GetOuputParams();

                return new ClosedLockedStatus(
                    Convert.ToBoolean(ouputParams["isactive"]),
                    Convert.ToByte(ouputParams["locked"]));
            }
        }

        public bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId }
            };
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);

            var isBelonging = false;
            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_OLD"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                isBelonging = (short)outputs["roleid"] > 0;
            }

            return isBelonging;
        }

        public BelongingResult Age_CustomerSelCheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId }
            };
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);
            parameters.Add("isactive", true);

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                var roleId = Converter.To<byte>(outputs["roleid"]);

                return new BelongingResult(
                    roleId,
                    isInternal || roleId > 0 && Converter.ToBool(outputs["isactive"])
                );
            }
        }

        public BelongingResult Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(int custId, int currentCustId, int subAccountId, long transId, bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId }
            };
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);
            parameters.Add("isactive", true);

            using (var objectDb = new ObjectDb("Age_Rpt_CustomerConsolidatedGroup_CheckBelonging"))
            {
                objectDb.AssignParamValues(parameters);
                objectDb.ExecuteNonQuery();
                var outputs = objectDb.GetOuputParams();
                var roleId = Converter.To<byte>(outputs["roleid"]);

                return new BelongingResult(
                    roleId,
                    isInternal || roleId > 0 && Converter.ToBool(outputs["isactive"])
                );
            }
        }

        public BelongingResult IsMultipleCustomersBelonging(string custIds, int currentCustId, int subAccountId, bool isInternal)
        {
            var parameters = new Hashtable
            {
                { "sessioncustid", currentCustId },
                { "subaccountid", subAccountId },
                { "custid", custIds },
                { "roleid", 0 },
                { "isactive", true }
            };

            using (var objectDb = new ObjectDb("Age_CustomerSelCheckBelonging_MultiCust"))
            {
                objectDb.ExecuteNonQuery(parameters);
                var outputs = objectDb.GetOuputParams();
                var roleId = Converter.To<byte>(outputs["roleid"]);

                return new BelongingResult(
                    roleId,
                    isInternal || roleId > 0 && Converter.ToBool(outputs["isactive"])
                );
            }
        }
    }
}
