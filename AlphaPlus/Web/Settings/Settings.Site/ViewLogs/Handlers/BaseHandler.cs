// namespace AlphaPlus.Settings.Site.ViewLogs.Handlers
// {
//     using System;
//     using AlphaPlus.Settings.ViewLogs.API.Constants;
//     using AlphaPlus.Settings.ViewLogs.API.Helpers;
//     using AlphaPlus.Settings.ViewLogs.API.Params;
//     using AlphaPlus.Settings.ViewLogs.API.Services;
//     using Fanex.Common;
//     using Requests;

//     public class BaseHandler
//     {
//         public BaseHandler(
//             LogService logService,
//             LogFormatter logFormatter,
//             IDateTime dateTimeWrapper)
//         {
//             LogFormatter = logFormatter;
//             LogService = logService;
//             DateTimeWrapper = dateTimeWrapper;
//         }

//         public LogFormatter LogFormatter { get; }

//         public LogService LogService { get; }

//         public IDateTime DateTimeWrapper { get; }

//         protected LogParam CreateLogParam(LogQuery logQuery)
//         {
//             var fromDate = GetFromDate(logQuery);
//             var toDate = GetToDate(logQuery);
//             var pageSize = string.IsNullOrWhiteSpace(logQuery.PageSize)
//                 ? DefaultVariables.PageSize
//                 : Converter.ToInt(logQuery.PageSize);
//             return new LogParam
//             {
//                 UplineId = logQuery.UplineId,
//                 SubAccId = logQuery.SubAccId,
//                 RoleId = logQuery.RoleId,
//                 ActorId = logQuery.ActorId,
//                 FromDate = fromDate,
//                 ToDate = toDate,
//                 Username = logQuery.Username,
//                 IsInternal = logQuery.IsInternal,

//                 PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize,
//                 PageIndex = string.IsNullOrWhiteSpace(logQuery.PageIndex)
//                 ? 0
//                 : Converter.ToInt(logQuery.PageIndex),

//                 Language = logQuery.Language,
//                 HasViewLogPermission = logQuery.HasViewLogPermission,

//                 MaxReportDate = logQuery.MaxReportDate,
//                 MinReportDate = logQuery.MinReportDate,
//                 IsShowOldValue = logQuery.IsShowOldValue,
//                 LimitedDateToShowInfo = logQuery.LimitedDateForShowInfo
//             };
//         }

//         protected DateTime GetFromDate(LogQuery logQuery)
//         {
//             if (logQuery == null || string.IsNullOrWhiteSpace(logQuery.FromDate))
//             {
//                 return DateTimeWrapper.Now.Day > 1
//                     ? DateTimeWrapper.Now.AddDays(-1)
//                     : DateTimeWrapper.Now;
//             }

//             return Converter.ToDateTime(logQuery.FromDate);
//         }

//         protected DateTime GetToDate(LogQuery logQuery)
//         {
//             return logQuery == null || string.IsNullOrWhiteSpace(logQuery.ToDate)
//                 ? DateTimeWrapper.Now
//                 : Converter.ToDateTime(logQuery.ToDate);
//         }
//     }
// }