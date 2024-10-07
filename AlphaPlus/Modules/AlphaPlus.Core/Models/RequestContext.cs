using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Core.Models;

public interface IRequestContext
{
    string Url { get; }

    long TransId { get; }

    bool IsDataStoreInDataWarehouse { get; }

    int CustId { get; }

    string CustIds { get; }

    IUserSession UserSession { get; }
}

public class RequestContext(IRequestAnalyst requestAnalyst, IOptions<AuthorizationOptions> options) : IRequestContext
{
    private string _url;
    private int _custId = 0;
    private string _custIds = string.Empty;
    private readonly IRequestAnalyst requestAnalyst = requestAnalyst;
    private readonly AuthorizationOptions options = options.Value;

    public string Url
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_url))
            {
                _url = AlphaContext.Current.Request.Path;
            }

            return _url;
        }
    }

    public long TransId
    {
        get
        {
            var query = AlphaContext.Current.Request.Query;

            _ = long.TryParse(query["transid"].ToString(), out long transId);

            if (transId == 0)
            {
                _ = long.TryParse(query["refno"].ToString(), out transId);
            }

            return transId;
        }
    }

    public bool IsDataStoreInDataWarehouse => AlphaContext.Current.Request.Query["iswhcheck"].ToString() == "1";

    public int CustId
    {
        get
        {
            if (_custId > 0)
            {
                return _custId;
            }

            var custIdQueryStringParamNames = options.CustIdQueryStringParamNames;
            var custIdJsonParamNames = options.CustIdJsonParamNames;
            var request = AlphaContext.Current.Request;

            foreach (string custIdQueryStringParamName in custIdQueryStringParamNames)
            {
                if (string.IsNullOrWhiteSpace(custIdQueryStringParamName))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(request.Query[custIdQueryStringParamName]))
                {
                    int.TryParse(request.Query[custIdQueryStringParamName], out _custId);
                    if (_custId > 0)
                    {
                        break;
                    }

                    if (request.HasFormContentType)
                    {
                        int.TryParse(request.Form[custIdQueryStringParamName], out _custId);
                        if (_custId > 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (_custId == 0)
            {
                foreach (string custIdJsonParamName in custIdJsonParamNames)
                {
                    _custId = requestAnalyst.GetCustomerId(custIdJsonParamName);

                    if (_custId > 0)
                    {
                        break;
                    }
                }
            }

            return _custId;
        }
    }

    public string CustIds
    {
        get
        {
            if (!string.IsNullOrEmpty(_custIds))
            {
                return _custIds;
            }

            var custIdsQueryStringParamNames = options.CustIdsQueryStringParamNames;
            var request = AlphaContext.Current.Request;

            foreach (string custIdQueryStringParamName in custIdsQueryStringParamNames)
            {
                if (string.IsNullOrWhiteSpace(custIdQueryStringParamName))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(request.Query[custIdQueryStringParamName].ToString()))
                {
                    _custIds = request.Query[custIdQueryStringParamName].ToString();
                    if (!string.IsNullOrEmpty(_custIds))
                    {
                        _custIds = _custIds.Replace(',', '^');
                        break;
                    }

                    _custIds = request.Form[custIdQueryStringParamName].ToString();
                    if (!string.IsNullOrEmpty(_custIds))
                    {
                        _custIds = _custIds.Replace(',', '^');
                        break;
                    }
                }
            }

            return _custIds;
        }
    }

    public IUserSession UserSession => Models.UserSession.Current;

}
