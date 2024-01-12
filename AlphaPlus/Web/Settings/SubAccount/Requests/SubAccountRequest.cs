using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Settings.Site.SubAccount.Models;
using EasyNetQ.Producer;
using MediatR;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public class SubAccountRequest : IRequest<IEnumerable<SubAccountViewModel>>
    {
        public int CustId { get; set; }

        public bool HasFullPermission { get; set; }

        public bool HasUpdatePermission { get; set; }

        public bool HasBetlistPermission { get; set; }

        public bool HasReportPermission { get; set; }

        public bool HasTotalBetsForecastPermission { get; set; }

        public bool HasViewPermission { get; set; }
    }
}