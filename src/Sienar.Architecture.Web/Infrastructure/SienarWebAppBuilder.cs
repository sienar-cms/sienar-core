using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Extensions;
using Sienar.Plugins;

namespace Sienar.Infrastructure;

/// <summary>
/// The Sienar web app builder, which is used to create Sienar applications
/// </summary>
public sealed class SienarWebAppBuilder
{
	/// <summary>
	/// The <see cref="WebApplicationBuilder"/> backing this Sienar application
	/// </summary>
	public readonly WebApplicationBuilder Builder;

	/// <summary>
	/// A list of middlewares to be applied to the app
	/// </summary>
	public readonly List<Action<WebApplication>> Middlewares = [];

	/// <summary>
	/// A dictionary that contains custom values that can be used by non-Sienar code during the application initialization process
	/// </summary>
	public readonly Dictionary<string, object> CustomItems = new();

	/// <summary>
	/// The plugin data provider added to the DI container
	/// </summary>
	public readonly IPluginDataProvider PluginDataProvider;

	/// <summary>
	/// The startup args as provided to <c>public static void Main(string[] args)</c>
	/// </summary>
	public string[] StartupArgs = Array.Empty<string>();

	private SienarWebAppBuilder(WebApplicationBuilder builder)
	{
		Builder = builder;
		PluginDataProvider = new PluginDataProvider();
		Builder.Services.AddSingleton(PluginDataProvider);
	}

	/// <summary>
	/// Creates a new <see cref="SienarWebAppBuilder"/> and registers core Sienar services on its service collection
	/// </summary>
	/// <param name="args">the runtime arguments supplied to <c>Program.Main()</c></param>
	/// <returns>the new <see cref="SienarWebAppBuilder"/></returns>
	public static SienarWebAppBuilder Create(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddSienarCoreUtilities();

		return new SienarWebAppBuilder(builder) { StartupArgs = args };
	}

	/// <summary>
	/// Adds an <see cref="IWebPlugin"/> to the Sienar app
	/// </summary>
	/// <typeparam name="TPlugin">the type of the plugin to add</typeparam>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder AddPlugin<TPlugin>()
		where TPlugin : IWebPlugin, new()
		=> AddPlugin(new TPlugin());

	/// <summary>
	/// Adds an instance of <see cref="IWebPlugin"/> to the Sienar app
	/// </summary>
	/// <param name="plugin">an instance of the plugin to add</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder AddPlugin(IWebPlugin plugin)
	{
		plugin.SetupDependencies(Builder);
		Middlewares.Add(plugin.SetupApp);
		PluginDataProvider.Add(plugin.PluginData);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplicationBuilder"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebApplicationBuilder}"/> that accepts the <see cref="WebApplicationBuilder"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder SetupDependencies(Action<WebApplicationBuilder> configurer)
	{
		configurer(Builder);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplication"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebApplication}"/> that accepts the <see cref="WebApplication"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder SetupApp(Action<WebApplication> configurer)
	{
		Middlewares.Add(configurer);
		return this;
	}

	/// <summary>
	/// Builds the final <see cref="WebApplication"/> and returns it
	/// </summary>
	/// <returns>the new <see cref="WebApplication"/></returns>
	public WebApplication Build()
	{
		var usesAuthorization = false;
		var usesAuthentication = false;
		var usesCors = false;

		// Configure auth
		var authorizationConfigurer = Builder.Services.GetService<IConfigurer<AuthorizationOptions>>();
		var authenticationConfigurer = Builder.Services.GetService<IConfigurer<AuthenticationOptions>>();
		var authenticationOptionsConfigurer = Builder.Services.GetService<IConfigurer<AuthenticationBuilder>>();

		if (authorizationConfigurer is not null)
		{
			usesAuthorization = true;
			Builder.Services.AddAuthorization(o => authorizationConfigurer.Configure(o, Builder.Configuration));
		}

		if (authenticationConfigurer is not null)
		{
			usesAuthentication = true;
			var authBuilder = Builder.Services.AddAuthentication(o => authenticationConfigurer.Configure(o, Builder.Configuration));
			authenticationOptionsConfigurer?.Configure(authBuilder, Builder.Configuration);
		}

		// Configure CORS
		var corsConfigurer = Builder.Services.GetService<IConfigurer<CorsOptions>>();
		if (corsConfigurer is not null)
		{
			usesCors = true;
			Builder.Services.AddCors(o => corsConfigurer.Configure(o, Builder.Configuration));
		}

		// Build the app
		var app = Builder.Build();

		// Set up middlewares
		if (!app.Environment.IsDevelopment())
		{
			app
				.UseExceptionHandler("/Error")
				.UseHsts();
		}

		app.UseStaticFiles();

		if (usesCors)
		{
			var corsMiddlewareConfigurer = app.Services.GetService<IConfigurer<CorsPolicyBuilder>>();
			if (corsMiddlewareConfigurer is not null)
			{
				app.UseCors(o => corsMiddlewareConfigurer.Configure(o, app.Configuration));
			}
			else
			{
				app.UseCors();
			}
		}

		app.UseRouting();

		if (usesAuthentication)
		{
			app.UseAuthentication();
		}

		if (usesAuthorization)
		{
			app.UseAuthorization();
		}

		foreach (var middleware in Middlewares)
		{
			middleware(app);
		}

		return app;
	}
}