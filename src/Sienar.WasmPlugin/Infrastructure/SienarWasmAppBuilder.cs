using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Extensions;
using Sienar.Plugins;

namespace Sienar.Infrastructure;

/// <summary>
/// The Sienar WASM app builder, which is used to create Sienar applications using WASM
/// </summary>
public class SienarWasmAppBuilder
{
	/// <summary>
	/// The <see cref="WebAssemblyHostBuilder"/> backing this Sienar application
	/// </summary>
	public readonly WebAssemblyHostBuilder Builder;

	/// <summary>
	/// The primary assembly to scan for routable components
	/// </summary>
	public readonly Assembly? AppAssembly;

	/// <summary>
	/// A list of middlewares to be applied to the app
	/// </summary>
	public readonly List<Action<WebAssemblyHost>> Middlewares = [];

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

	private SienarWasmAppBuilder(
		WebAssemblyHostBuilder builder,
		Assembly? appAssembly)
	{
		Builder = builder;
		AppAssembly = appAssembly;
		PluginDataProvider = new PluginDataProvider();
		Builder.Services
			.AddSingleton(PluginDataProvider)
			.AddSingleton(new HttpClient
			{
				DefaultRequestHeaders = {{ "X-Requested-With", "XMLHttpRequest" }}
			})
			.AddAuthorizationCore()
			.AddSienarCoreUtilities()
			.AddSienarBlazorUtilities()
			.AddCascadingAuthenticationState();
	}

	/// <summary>
	/// Creates a new <see cref="SienarWasmAppBuilder"/> and registers core Sienar services on its service collection
	/// </summary>
	/// <param name="args">the runtime arguments supplied to <c>Program.Main()</c></param>
	/// <returns>the new <see cref="SienarWasmAppBuilder"/></returns>
	public static SienarWasmAppBuilder Create<T>(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);
		return new SienarWasmAppBuilder(
			builder,
			typeof(T).Assembly)
		{
			StartupArgs = args
		};
	}

	/// <summary>
	/// Adds an <see cref="IWasmPlugin"/> to the Sienar app
	/// </summary>
	/// <typeparam name="TPlugin">the type of the plugin to add</typeparam>
	/// <returns>the Sienar app builder</returns>
	public SienarWasmAppBuilder AddPlugin<TPlugin>()
		where TPlugin : IWasmPlugin, new()
		=> AddPlugin(new TPlugin());

	/// <summary>
	/// Adds an instance of <see cref="IWasmPlugin"/> to the Sienar app
	/// </summary>
	/// <param name="plugin">an instance of the plugin to add</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWasmAppBuilder AddPlugin(IWasmPlugin plugin)
	{
		plugin.SetupDependencies(Builder);
		Middlewares.Add(plugin.SetupApp);
		PluginDataProvider.Add(plugin.PluginData);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebAssemblyHostBuilder"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebAssemblyHostBuilder}"/> that accepts the <see cref="WebAssemblyHostBuilder"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWasmAppBuilder SetupDependencies(Action<WebAssemblyHostBuilder> configurer)
	{
		configurer(Builder);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebAssemblyHost"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebAssemblyHost}"/> that accepts the <see cref="WebAssemblyHost"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWasmAppBuilder SetupApp(Action<WebAssemblyHost> configurer)
	{
		Middlewares.Add(configurer);
		return this;
	}

	/// <summary>
	/// Adds a root component to the application
	/// </summary>
	/// <param name="selector">the CSS selector to render the component at</param>
	/// <typeparam name="TComponent">the type of the component</typeparam>
	/// <returns>the Sienar app builder</returns>
	public SienarWasmAppBuilder AddComponent<TComponent>(string selector)
		where TComponent : IComponent
	{
		Builder.RootComponents.Add<TComponent>(selector);
		return this;
	}

	/// <summary>
	/// Builds the final <see cref="WebAssemblyHost"/> and returns it
	/// </summary>
	/// <returns>the new <see cref="WebAssemblyHost"/></returns>
	public WebAssemblyHost Build()
	{
		var app = Builder.Build();

		if (AppAssembly is not null)
		{
			app.Services.ConfigureRoutableAssemblies(p => p.Add(AppAssembly));
		}

		foreach (var middleware in Middlewares)
		{
			middleware(app);
		}

		return app;
	}
}