namespace Sunplus.Agent.Model.Transfer
{
    using System;
    using System.Collections;
    using System.Data;
    using AlphaPlus.User.Models;
    using Fanex.Data.DNAide;

    public class TransferBO 
    {
        public DataTable GetSuperInAutomateTransferAll(int custId)
        {
            var dao = new TransferDAO();

            return dao.GetSuperInAutomateTransferAll(custId);
        }

        public int UpdateTransferYesBalance(
            string custId,
            double amt,
            bool isFullTransfer,
            bool isChecked,
            string status,
            int autoTransferFilter,
            string ip,
            out string errMsg)
        {
            if (!isChecked)
            {
                status = string.Empty;
            }

            TransferDAO dao = new TransferDAO();

            if (UserSession.Current.IsAgent)
            {
                var isAllDayBalanceTransfer = dao.IsAllDayBalanceTransfer(UserSession.Current.CustId);
                return dao.Age_Transfer_Ins_Ag(
                       custId,
                       UserSession.Current.UserName,
                       UserSession.Current.CustId,
                       UserSession.Current.SubAccId,
                       ip,
                       -amt,
                       isFullTransfer,
                       AppSettings.AllowedListSuperAccounts,
                       UserSession.Current.LangKey,
                       isAllDayBalanceTransfer,
                       autoTransferFilter,
                       out errMsg);
            }

            if (UserSession.Current.IsMaster)
            {
                return dao.Age_Transfer_Ins_MAg(
                    custId,
                    UserSession.Current.CustId,
                    UserSession.Current.SubAccId,
                    UserSession.Current.CustId,
                    ip,
                    -amt,
                    isFullTransfer,
                    status,
                    AppSettings.AllowedListSuperAccounts,
                    UserSession.Current.LangKey,
                    autoTransferFilter,
                    out errMsg);
            }

            if (UserSession.Current.IsSuper)
            {
                return dao.Age_Transfer_Ins_SAg(
                    custId,
                    UserSession.Current.CustId,
                    UserSession.Current.SubAccId,
                    UserSession.Current.CustId,
                    ip,
                    -amt,
                    isFullTransfer,
                    status,
                    AppSettings.AllowedListSuperAccounts,
                    UserSession.Current.LangKey,
                    autoTransferFilter,
                    out errMsg);
            }

            errMsg = string.Empty;

            return 0;
        }

        public int TransferYesCleareBalanceForAgent(string toCustIds, bool isChecked, string status, int autoTransferFilter, string ip, out string errMsg)
        {
            TransferDAO dao = new TransferDAO();

            if (!isChecked)
            {
                status = string.Empty;
            }

            var isAllDayBalanceTransfer = dao.IsAllDayBalanceTransfer(UserSession.Current.CustId);
            return dao.Age_Transfer_InsClearBalance_Ag(
                toCustIds,
                UserSession.Current.SubAccId,
                UserSession.Current.UserId,
                UserSession.Current.CustId,
                ip,
                status,
                UserSession.Current.LangKey,
                AppSettings.AllowedListSuperAccounts,
                isAllDayBalanceTransfer,
                autoTransferFilter,
                out errMsg);
        }

        public DataTable GetDataTransfer(int status, string userName, bool isYesterdayBalance, int autoTransferFilter)
        {
            int totalPages = 0;
            return GetDataTransfer(status, 1, ref totalPages, int.MaxValue - 1, userName, isYesterdayBalance, autoTransferFilter);
        }

        public DataTable GetDataTransfer(int status, int pageIndex, ref int totalPages, string userName, bool isYesterdayBalance, int autoTransferFilter)
        {
            return GetDataTransfer(status, pageIndex, ref totalPages, AppSettings.PageSize, userName, isYesterdayBalance, autoTransferFilter);
        }

        public DataTable GetDataTransfer(int status, int pageIndex, ref int totalPages, int pagesize, string userName, bool isYesterdayBalance, int autoTransferFilter)
        {
            TransferDAO dao = new TransferDAO();

            string custStatus = string.Empty;

            switch (status)
            {
                case 1:
                    custStatus = "Open";
                    break;
                case 2:
                    custStatus = "Suspended";
                    break;
                case 3:
                    custStatus = "Closed";
                    break;
                case 4:
                    custStatus = "Disabled";
                    break;
                default:
                    custStatus = "All";
                    break;
            }

            if (UserSession.Current.IsAgent)
            {
                return dao.Age_Transfer_SelList_Ag(
                    UserSession.Current.CustId,
                    custStatus,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    UserSession.Current.IsAllowedTotalBalanceTransfer,
                    autoTransferFilter);
            }

            if (UserSession.Current.IsMaster)
            {
                return dao.Age_Transfer_SelList_MAg(
                    UserSession.Current.CustId,
                    custStatus,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    UserSession.Current.IsAllowedTotalBalanceTransfer,
                    autoTransferFilter);
            }

            if (UserSession.Current.IsSuper)
            {
                return dao.Age_Transfer_SelList_SAg(
                    UserSession.Current.CustId,
                    custStatus,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    UserSession.Current.IsAllowedTotalBalanceTransfer,
                    autoTransferFilter);
            }

            return null;
        }

        // start Jeff add for Member Info 281011
        public Hashtable TransferSelMemberInfoYesterday(string custId)
        {
            TransferDAO dao = new TransferDAO();

            return UserSession.Current.IsAgent
                ? dao.Age_Transfer_SelMemberInfo_Yesterday(
                    custId,
                    UserSession.Current.IsAllowedTotalBalanceTransfer)
                : null;
        }

        // start Jeff add Check for existing transfer 081111
        public DataTable TransferCheckExistingForAgent(string custId)
        {
            TransferDAO dao = new TransferDAO();

            return UserSession.Current.IsAgent
                ? dao.Age_Transfer_CheckExisting(
                    custId)
                : null;
        }

        public string GetOptionTransferCondition(int custId)
        {
            TransferDAO dao = new TransferDAO();
            return dao.Acc_CustomerSelTransferOption(custId, string.Empty);
        }

        public string GetTransferCondition(int custId)
        {
            TransferDAO dao = new TransferDAO();
            return dao.Acc_CustomerSelTransferOption(custId, "00000000");
        }

        public DbResult UpdateTransferCondition(string custId, string transferOption, string autoTransferOption, string ip)
        {
            TransferDAO dao = new TransferDAO();

            return dao.Acc_CustomerUpdTransferOption(
                custId,
                transferOption,
                autoTransferOption,
                UserSession.Current.UserId.ToString(),
                UserSession.Current.SubAccId,
                UserSession.Current.LangKey,
                ip);
        }

        public bool IsAllDayBalanceTransfer(int custId)
            => new TransferDAO().IsAllDayBalanceTransfer(custId);
    }
}