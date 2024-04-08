using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPlus.Features.Messages.API.Models
{
    public class PagedReportData<T> where T : class
    {
        public PagedReportData(
            IEnumerable<T> dataSource,
            int pageIndex,
            int pageSize,
            int totalRecords)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            DataSource = dataSource;
        }

        public IEnumerable<T> DataSource { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public bool Any() => TotalRecords > 0;

        public bool MoreThanOne() => TotalRecords > 1;
    }
}
