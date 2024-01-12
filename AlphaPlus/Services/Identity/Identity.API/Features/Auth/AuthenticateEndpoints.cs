namespace Identity.API.Features.Auth
{
    public static class AuthenticateEndpoints
    {
        public static void RegisterAuthenticateEndpoints(this WebApplication app)
        {
            var auth = app.MapGroup("/auth");

            auth.MapPost("/sso", (string token) => { });
        }
    }
}