using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Configures server-side rendered ASP.NET architecture in a Sienar application, such as ASP.NET controllers with views and convention-based routing
/// </summary>
public class Ssr : IWebPlugin
{
	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar SSR Plugin",
		Description = "Adds Sienar functionality as a traditional server-side web app",
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Homepage = "https://sienar.levesque.dev",
		Version = Version.Parse("0.1.0")
	};

	/// <inheritdoc />
	public void SetupDependencies(WebApplicationBuilder builder)
	{
		builder.Services.AddControllersWithViews();
		builder.Services
			.AddScoped<IReadableNotificationService, RestNotificationService>()
			.AddScoped<INotificationService>(
				sp => sp.GetRequiredService<IReadableNotificationService>());
	}

	/// <inheritdoc />
	public void SetupApp(WebApplication app)
	{
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");
	}
}