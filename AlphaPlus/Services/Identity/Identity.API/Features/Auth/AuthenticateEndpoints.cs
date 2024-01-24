using Microsoft.AspNetCore.Http.HttpResults;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public static class AuthenticateEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var auth = endpoints.MapGroup("/api/auth")
                .WithTags("Auth")
                .WithOpenApi();

            auth.MapPost("/after", AfterSignIn);
            auth.MapPost("/check-in", CheckIn);

            return endpoints;
        }

        static async Task<Results<Ok<AuthenticateResponse>, BadRequest>> AfterSignIn(AuthenticateRequest request, IAuthenticateService service)
        {
            var response = await service.LoginClaimDownline(request);
            return TypedResults.Ok(response);
        }

        static async Task<Results<Ok<int>, BadRequest>> CheckIn(CheckInRequest request, IAuthenticateService service)
        {
            var onlineUserId = await service.CheckIn(request);
            return TypedResults.Ok(onlineUserId);
        }
    }
}