using BlazorApp1.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure logging for Azure with more details
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configure for Azure Web App Linux Container
var port = Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? 
           Environment.GetEnvironmentVariable("PORT") ?? "80";

Console.WriteLine($"[STARTUP] Configuring port: {port}");
Console.WriteLine($"[STARTUP] Environment: {builder.Environment.EnvironmentName}");

// Simple URL configuration for Azure Web App
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Log startup information immediately
Console.WriteLine("=== APPLICATION STARTING ===");
Console.WriteLine($"Port: {port}");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Removed app.UseHsts() for Docker/Azure Web App compatibility
    // Azure handles SSL termination at the load balancer level
}

app.UseAntiforgery();
app.UseStaticFiles();

// Add health check endpoints for Azure (removed conflicting root route)
app.MapGet("/health", () => "Healthy");
app.MapGet("/ready", () => "Ready");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

Console.WriteLine("=== APPLICATION CONFIGURED, STARTING SERVER ===");

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR] Application failed to start: {ex.Message}");
    Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
    throw;
}
