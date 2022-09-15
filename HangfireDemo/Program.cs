using Hangfire;
using Hangfire.Dashboard;
using HangfireDemo.Filters;
using HangfireDemo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage()
        );

builder.Services.AddHangfireServer(options =>
{
    options.CancellationCheckInterval = TimeSpan.FromSeconds(1);
    options.WorkerCount = Environment.ProcessorCount * 2;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseHangfireDashboard(options: new DashboardOptions { Authorization = new List<IDashboardAuthorizationFilter> { new HangfireAuthorizationFilter() } });

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHangfireDashboard();
});

BackgroundJob.Schedule<HangfireJobsSchedular>(e => e.AddHangfireJobs(), TimeSpan.FromMinutes(10));

app.Run();
