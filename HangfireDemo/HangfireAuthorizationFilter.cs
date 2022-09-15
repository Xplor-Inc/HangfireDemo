using Hangfire.Dashboard;

namespace HangfireDemo.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
       return true;
    }
}