using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Threading.RateLimiting;
using System.Text.Json;

namespace AlphaPlus.NetThrottling
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimiterMiddleware> _logger;
        private readonly PartitionedRateLimiter<HttpContext> _rateLimiter;
        private readonly RateLimiterConfiguration _config;

        public RateLimiterMiddleware(
            RequestDelegate next, 
            ILogger<RateLimiterMiddleware> logger,
            IOptions<RateLimiterConfiguration> config)
        {
            _next = next;
            _logger = logger;
            _config = config.Value;
            _rateLimiter = CreateRateLimiter();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if endpoint is disabled for rate limiting
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IDisableNetThrottling>() is object || 
                context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            // Apply rate limiting
            using var lease = await _rateLimiter.AcquireAsync(context);
            
            if (!lease.IsAcquired)
            {
                _logger.LogWarning("Rate limit exceeded for {UserIdentifier} on {Path}", 
                    GetUserIdentifier(context), context.Request.Path);
                
                await HandleRateLimitExceeded(context, lease);
                return;
            }

            await _next(context);
        }

        private PartitionedRateLimiter<HttpContext> CreateRateLimiter()
        {
            return PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                // Get user identifier (IP + potential customer ID)
                var identifier = GetUserIdentifier(context);
                
                // Check whitelist
                if (IsWhitelisted(context))
                {
                    return RateLimitPartition.GetNoLimiter(identifier);
                }

                // Apply different policies based on request type
                var policyName = GetPolicyName(context);
                
                return policyName switch
                {
                    "NewCustomer" => RateLimitPartition.GetFixedWindowLimiter(
                        $"nc_{identifier}", 
                        _ => new FixedWindowRateLimiterOptions
                        {
                            Window = TimeSpan.FromSeconds(_config.NewCustomerPolicy.ThrottleDuration),
                            PermitLimit = _config.NewCustomerPolicy.MaxRequests,
                            QueueLimit = 0
                        }),
                    
                    "Main" => RateLimitPartition.GetSlidingWindowLimiter(
                        $"main_{identifier}", 
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            Window = TimeSpan.FromSeconds(_config.MainPolicy.ThrottleDuration),
                            PermitLimit = _config.MainPolicy.MaxRequests,
                            SegmentsPerWindow = 4,
                            QueueLimit = 5
                        }),
                    
                    _ => RateLimitPartition.GetFixedWindowLimiter(
                        $"any_{identifier}", 
                        _ => new FixedWindowRateLimiterOptions
                        {
                            Window = TimeSpan.FromSeconds(_config.AnyPolicy.ThrottleDuration),
                            PermitLimit = _config.AnyPolicy.MaxRequests,
                            QueueLimit = 2
                        })
                };
            });
        }

        private string GetUserIdentifier(HttpContext context)
        {
            // Try to get customer ID from session/claims/headers
            var custId = GetCustomerId(context);
            if (custId.HasValue)
            {
                return $"cust_{custId.Value}";
            }

            // Fallback to IP address with X-Forwarded-For support
            var ip = GetClientIpAddress(context);
            return $"ip_{ip}";
        }

        private string GetClientIpAddress(HttpContext context)
        {
            // Check X-Forwarded-For header first
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                var ip = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
                if (!string.IsNullOrEmpty(ip))
                {
                    return ip;
                }
            }

            // Check X-Real-IP header
            if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
            {
                var ip = realIp.FirstOrDefault()?.Trim();
                if (!string.IsNullOrEmpty(ip))
                {
                    return ip;
                }
            }

            // Fallback to connection remote IP
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private int? GetCustomerId(HttpContext context)
        {
            // Try multiple sources for customer ID
            
            // From claims
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var custIdClaim = context.User.FindFirst("custId")?.Value ??
                                 context.User.FindFirst("CustomerId")?.Value ??
                                 context.User.FindFirst("customer_id")?.Value;
                
                if (int.TryParse(custIdClaim, out var custId))
                {
                    return custId;
                }
            }

            // From session
            if (context.Session.TryGetValue("custId", out var sessionValue))
            {
                if (int.TryParse(System.Text.Encoding.UTF8.GetString(sessionValue), out var custId))
                {
                    return custId;
                }
            }

            // From headers (if API)
            if (context.Request.Headers.TryGetValue("X-Customer-Id", out var headerValue))
            {
                if (int.TryParse(headerValue.FirstOrDefault(), out var custId))
                {
                    return custId;
                }
            }

            // From query parameters
            if (context.Request.Query.TryGetValue("custId", out var queryValue))
            {
                if (int.TryParse(queryValue.FirstOrDefault(), out var custId))
                {
                    return custId;
                }
            }

            return null;
        }

        private bool IsWhitelisted(HttpContext context)
        {
            var custId = GetCustomerId(context);
            return custId.HasValue && _config.WhiteListCustIds.Contains(custId.Value);
        }

        private string GetPolicyName(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

            // NewCustomer policy - registration/signup endpoints
            if (method == "POST" && (
                path.Contains("/register") || 
                path.Contains("/signup") || 
                path.Contains("/create") ||
                path.Contains("/newcustomer") ||
                path.Contains("/customer/new")))
            {
                return "NewCustomer";
            }

            // Main policy - all POST requests
            if (method == "POST")
            {
                return "Main";
            }

            // Default policy for all other requests (Any)
            return "Any";
        }

        private async Task HandleRateLimitExceeded(HttpContext context, RateLimitLease lease)
        {
            context.Response.StatusCode = 429; // Too Many Requests
            
            // Add retry-after header if available
            if (lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
            }

            // Add rate limit headers for debugging
            context.Response.Headers.Add("X-RateLimit-Policy", GetPolicyName(context));
            context.Response.Headers.Add("X-RateLimit-Identifier", GetUserIdentifier(context));
            context.Response.Headers.Add("X-RateLimit-Remaining", "0");

            var policyName = GetPolicyName(context);
            var blockDuration = policyName switch
            {
                "NewCustomer" => _config.NewCustomerPolicy.BlockDuration,
                "Main" => _config.MainPolicy.BlockDuration,
                _ => _config.AnyPolicy.BlockDuration
            };

            var response = new
            {
                error = "Rate limit exceeded",
                message = "Too many requests. Please try again later.",
                statusCode = 429,
                policy = policyName,
                retryAfterSeconds = blockDuration
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }

        public void Dispose()
        {
            _rateLimiter?.Dispose();
        }
    }

    // Marker interface for disabling rate limiting (compatibility with existing code)
    public interface IDisableNetThrottling
    {
    }

    // Attribute for disabling rate limiting
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableRateLimitingAttribute : Attribute, IDisableNetThrottling
    {
    }

    // Configuration classes
    public class RateLimiterConfiguration
    {
        public PolicyConfiguration AnyPolicy { get; set; } = new();
        public PolicyConfiguration MainPolicy { get; set; } = new();
        public PolicyConfiguration NewCustomerPolicy { get; set; } = new();
        public int[] WhiteListCustIds { get; set; } = Array.Empty<int>();
    }

    public class PolicyConfiguration
    {
        public int MaxRequests { get; set; }
        public int ThrottleDuration { get; set; } // in seconds
        public int BlockDuration { get; set; } // in seconds
    }
}
