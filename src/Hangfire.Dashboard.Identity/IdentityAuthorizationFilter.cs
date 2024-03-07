namespace Hangfire.Dashboard.Identity;

public class IdentityAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly IdentityAuthorizationFilterOptions options;

    public IdentityAuthorizationFilter(IdentityAuthorizationFilterOptions options)
    {
        this.options = options;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
        {
            // If not authenticated, redirect to the login page.
            var originalPath = httpContext.Request.Path + httpContext.Request.QueryString;
            var loginPath = $"{options.SignInPath}?ReturnUrl={Uri.EscapeDataString(originalPath)}";

            httpContext.Response.Redirect(loginPath);

            return false;
        }

        return true;
    }
}
