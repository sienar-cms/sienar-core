using System;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceProvider"/> extension methods for the <c>Sienar.Utils.Blazor</c> assembly
/// </summary>
public static class SienarUtilsBlazorServiceProviderExtensions
{
	/// <summary>
	/// Configures the <see cref="IRoutableAssemblyProvider">routable assembly provider</see>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">an <see cref="Action{IRoutableAssemblyProvider}">action</see> that configures the routable assembly provider</param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureRoutableAssemblies(
		this IServiceProvider self,
		Action<IRoutableAssemblyProvider> configurer)
		=> self.Configure(configurer);

	/// <summary>
	/// Configures the <see cref="IComponentProvider">component provider</see>
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="configurer">an <see cref="Action{IComponentProvider}">action</see> that configures the component provider</param>
	/// <returns>the service provider</returns>
	public static IServiceProvider ConfigureComponents(
		this IServiceProvider self,
		Action<IComponentProvider> configurer)
		=> self.Configure(configurer);
}