using Hangfire.Dashboard;

public class HangfireCustomBasicAuthenticationFilter : IDashboardAuthorizationFilter
{
    public string User { get; set; }
    public string Pass { get; set; }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Allow only local requests for security purposes
        if (httpContext.User.Identity.IsAuthenticated)
        {
            return true;
        }

        // Get the basic authentication header
        string authHeader = httpContext.Request.Headers["Authorization"];
        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            // Extract credentials
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var usernamePassword = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var parts = usernamePassword.Split(':');
            var username = parts[0];
            var password = parts[1];

            return username == User && password == Pass;
        }

        // No authentication header provided
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic";
        httpContext.Response.StatusCode = 401;
        return false;
    }
}
