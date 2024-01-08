using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Settings.Site.SubAccount.Models;
using AlphaPlus.Settings.Site.SubAccount.Requests;

namespace AlphaPlus.Settings.Site.SubAccount.Services
{
    public interface ISubAccountService
    {
        Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountRequest subAccountRequest);
    }

    public class SubAccountService : ISubAccountService
    {
        public async Task<IEnumerable<SubAccountViewModel>> GetSubAccounts(SubAccountRequest subAccountRequest)
        {
            throw new NotImplementedException();
        }
    }
}