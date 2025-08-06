using BlazorApp1.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure logging for Azure
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddDebug();
}

// Configure for Azure Web App Linux Container - improved port handling
var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? 
           Environment.GetEnvironmentVariable("PORT") ?? "80";

// Force binding to all interfaces for container environments
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== APPLICATION STARTING ===");
logger.LogInformation($"Port: {port}");
logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
logger.LogInformation($"Content Root: {app.Environment.ContentRootPath}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();


app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add health check endpoint for Azure
app.MapGet("/health", () => "Healthy");

// Log that application is ready
logger.LogInformation("=== APPLICATION READY ===");

// Graceful shutdown handling
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    logger.LogInformation("=== APPLICATION STOPPING ===");
});

app.Run();
