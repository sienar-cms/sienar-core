using System;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceProvider"/> extension methods for the <c>Sienar.Utils</c> assembly
/// </summary>
public static class SienarUtilsServiceProviderExtensions
{
	/// <summary>
	/// Performs an action against a given <c>TService</c>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">the action to execute against the <c>TService</c></param>
	/// <typeparam name="TService">the type of the service to configure</typeparam>
	/// <returns>the service provider</returns>
	public static IServiceProvider Configure<TService>(
		this IServiceProvider self,
		Action<TService> configurer)
		where TService : notnull
	{
		var provider = self.GetRequiredService<TService>();
		configurer(provider);
		return self;
	}

	/// <summary>
	/// Configures the app's <see cref="IMenuProvider"/>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">the action to execute against the <see cref="IMenuProvider"/></param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureMenus(
		this IServiceProvider self,
		Action<IMenuProvider> configurer)
		=> Configure(self, configurer);

	/// <summary>
	/// Configures the app's <see cref="IDashboardProvider"/>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">the action to execute against the <see cref="IDashboardProvider"/></param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureDashboards(
		this IServiceProvider self,
		Action<IDashboardProvider> configurer)
		=> Configure(self, configurer);

	/// <summary>
	/// Configures the app's <see cref="IScriptProvider"/>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">the action to execute against the <see cref="IScriptProvider"/></param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureScripts(
		this IServiceProvider self,
		Action<IScriptProvider> configurer)
		=> Configure(self, configurer);

	/// <summary>
	/// Configures the app's <see cref="IStyleProvider"/>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">the action to execute against the <see cref="IStyleProvider"/></param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureStyles(
		this IServiceProvider self,
		Action<IStyleProvider> configurer)
		=> Configure(self, configurer);
}