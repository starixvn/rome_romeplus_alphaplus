// using AlphaPlus.Reports.Site.Features.Shared.Requests;
// using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

// namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels;

// public abstract class DrillDownableViewModel<T> where T : class
// {
//     protected DrillDownableViewModel(
//         BaseQuery message,
//         BreadcrumbViewModel breadcrumbLevel,
//         string defaultBetListPath = null)
//     {
//         CustId = message.CustId;
//         CustName = message.CustName;
//         CustLevel = message.CustLevel;
//         BreadcrumbLevel = breadcrumbLevel;
//         BetListPath = defaultBetListPath;
//     }

//     public int CustId { get; }

//     public string CustName { get; }

//     public UserLevel CustLevel { get; }

//     public BreadcrumbViewModel BreadcrumbLevel { get; }

//     protected string BetListPath { get; set; }

//     protected abstract IDictionary<string, string> GetDownlineParams(T model);

//     protected abstract IDictionary<string, string> GetBetListParams(T model);
// }
