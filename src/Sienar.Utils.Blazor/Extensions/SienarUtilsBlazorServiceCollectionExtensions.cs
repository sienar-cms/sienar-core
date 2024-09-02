using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceCollection"/> extension methods for the <c>Sienar.Utils.Blazor</c> assembly
/// </summary>
public static class SienarUtilsBlazorServiceCollectionExtensions
{
	/// <summary>
	/// Adds Blazor-specific services to the service collection
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddSienarBlazorUtilities(this IServiceCollection self)
	{
		self.TryAddSingleton<IRoutableAssemblyProvider, RoutableAssemblyProvider>();
		self.TryAddSingleton<IComponentProvider, ComponentProvider>();
		self.TryAddSingleton<AuthenticationStateProvider, AuthStateProvider>();
		self.TryAddSingleton<IUserAccessor, BlazorUserAccessor>();

		return self;
	}
}