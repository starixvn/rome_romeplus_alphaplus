// namespace AlphaPlus.Features.Messages.Api.Filter
// {
//     using System;
//     using System.Net;
//     using System.Net.Http;
//     using System.Text;
//     using System.Web.Http.Filters;
//     using Fanex.Logging;

//     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
//     public sealed class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
//     {
//         public override void OnException(HttpActionExecutedContext actionExecutedContext)
//         {
//             if (actionExecutedContext != null)
//             {
//                 var exception = actionExecutedContext.Exception;
//                 var messageBuilder = new StringBuilder();

//                 messageBuilder.AppendFormat("Message: {0}", exception.Message).AppendLine();
//                 messageBuilder.AppendFormat("Type: {0}", exception.GetType().ToString()).AppendLine();
//                 messageBuilder.AppendFormat("Source: {0}", exception.Source).AppendLine();
//                 messageBuilder.AppendFormat("Url: {0}", actionExecutedContext.Request.RequestUri.OriginalString).AppendLine();

//                 // Log here
//                 Logger.Log.Error(exception.Message, exception);

//                 actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, messageBuilder.ToString());
//             }

//             base.OnException(actionExecutedContext);
//         }
//     }
// }