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
            auth.MapPost("/check-out", CheckOut);
            auth.MapGet("/re-check-in", ReCheckIn);
            auth.MapGet("/internal-login", InternalLogin);
            auth.MapGet("/indirect-login", IndirectLogin);

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

        static async Task<Results<Ok<ReCheckInResponse>, BadRequest>> ReCheckIn(int onlineUserId, IAuthenticateService service)
        {
            var response = await service.ReCheckIn(onlineUserId);
            return TypedResults.Ok(response);
        }


        static Task CheckOut(CheckOutRequest request, IAuthenticateService service)
            => service.CheckOut(request);

        static async Task<Results<Ok<InternalLoginResponse>, BadRequest>> InternalLogin([AsParameters]InternalLoginRequest request, IAuthenticateService service)
        {
            var response = await service.InternalLogin(request);
            return TypedResults.Ok(response);
        }

        static async Task<Results<Ok<IndirectLoginResponse>, BadRequest>> IndirectLogin([AsParameters]IndirectLoginRequest request, IAuthenticateService service)
        {
            var response = await service.IndirectLogin(request);
            return TypedResults.Ok(response);
        }
    }
}