using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using AlphaPlus.User.Models;
using Fanex.Products.Enums;
using Fanex.Products.Utilities;

namespace Fanex.Products.Services
{
    public abstract class BaseProduct : IProduct
    {
        private bool _active;

        private string _customerSettingLink;
        private string _enableStatusLink;
        private string _syncCustomerLink;

        public BaseProduct(bool webConfigActive)
        {
            _active = webConfigActive;
        }

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string RemotingRefName
        {
            get
            {
                return RefDataUtility.GetProductName(Id);
            }
        }

        public int SportId { get; set; }

        public string ReportSPParam { get; set; }

        public string ViewPTLink { get; set; }

        public string ViewCommLink { get; set; }

        public string MicroSiteNameAppSetting { get; set; }

        public string CustomerSettingLink
        {
            get
            {
                return GetAppPath(_customerSettingLink);
            }

            set
            {
                _customerSettingLink = value;
            }
        }

        public string EnableStatusLink
        {
            get
            {
                return GetAppPath(_enableStatusLink);
            }

            set
            {
                _enableStatusLink = value;
            }
        }

        public string SyncCustomerLink
        {
            get
            {
                return GetAppPath(_syncCustomerLink);
            }

            set
            {
                _syncCustomerLink = value;
            }
        }

        #endregion

        #region Virtual and Override Methods

        public virtual bool Active
        {
            get
            {
                return _active && !GetHide();
            }
        }

        public virtual bool Disabled
        {
            get
            {
                return GetDisabled();
            }
        }

        public virtual bool? Synced
        {
            get
            {
                return new bool?();
            }
        }

        public virtual bool ShowReport
        {
            get
            {
                return _active && !GetHide();
            }
        }

        public virtual bool ShowSettings
        {
            get
            {
                return _active && !GetHide();
            }
        }

        public virtual void SetProductStatusToSession(Hashtable outputParams)
        {
            string lowerName = Name.ToLowerInvariant();
            string storedProcedureName = "hide" + lowerName;

            if (outputParams.ContainsKey(storedProcedureName))
            {
                SetHide(Convert.ToBoolean(outputParams[storedProcedureName]));
            }

            storedProcedureName = "disable" + lowerName;

            if (outputParams.ContainsKey(storedProcedureName))
            {
                SetDisabled(Convert.ToBoolean(outputParams[storedProcedureName]));
            }
        }

        public abstract DCSIcon GetDCSIcon(DataRow drow);

        public abstract bool ShowDCSLinkForDownline(DataRow drow, int loggedInUserRole, int downlineRoleId);

        public abstract bool ShowEnableStatusLinkForDownline(DataRow drow);

        #endregion

        #region Session Methods

        protected bool GetHide()
        {
            return UserSession.Current.GetHideProduct(Id);
        }

        protected void SetHide(bool value)
        {
            UserSession.Current.SetHideProduct(Id, value);
        }

        protected bool GetDisabled()
        {
            return UserSession.Current.GetDisabledProduct(Id);
        }

        protected void SetDisabled(bool value)
        {
            UserSession.Current.SetDisabledProduct(Id, value);
        }

        protected virtual bool? GetSynced()
        {
            return UserSession.Current.GetSyncedProduct(Id);
        }

        protected void SetSynced(bool value)
        {
            UserSession.Current.SetSyncedProduct(Id, value);
        }

        #endregion

        private string GetAppPath(string pathModifier)
        {
            string appPath = HttpContext.Current.Response.ApplyAppPathModifier(pathModifier);

            return Regex.Replace(appPath, HttpRuntime.AppDomainAppVirtualPath, ConfigurationManager.AppSettings[MicroSiteNameAppSetting], RegexOptions.IgnoreCase);
        }
    }
}