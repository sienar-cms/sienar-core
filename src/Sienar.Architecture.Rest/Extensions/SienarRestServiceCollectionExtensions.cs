using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sienar.Data;
using Sienar.Services;

namespace Sienar.Extensions;

public static class SienarRestServiceCollectionExtensions
{
	/// <summary>
	/// Adds the necessary services to use an entity via a REST API repository
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TEntity">the type of the entity</typeparam>
	/// <typeparam name="TUrlProvider">the type of the REST URL provider</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddRestfulEntity<
		TEntity,
		TUrlProvider>(this IServiceCollection self)
		where TEntity : EntityBase
		where TUrlProvider : class, IRestfulRepositoryUrlProvider<TEntity>
		=> AddRestfulEntity<TEntity, TUrlProvider, RestfulRepository<TEntity>>(self);

	/// <summary>
	/// Adds the necessary services to use an entity via a REST API repository
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TEntity">the type of the entity</typeparam>
	/// <typeparam name="TUrlProvider">the type of the REST URL provider</typeparam>
	/// <typeparam name="TRepository">the implementation type of the repository</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddRestfulEntity<
		TEntity,
		TUrlProvider,
		TRepository>(this IServiceCollection self)
		where TUrlProvider : class, IRestfulRepositoryUrlProvider<TEntity>
		where TRepository : class, IRepository<TEntity>
		=> AddRestfulEntity<TEntity, TUrlProvider, IRepository<TEntity>, TRepository>(self);

	/// <summary>
	/// Adds the necessary services to use an entity via a REST API repository
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <typeparam name="TEntity">the type of the entity</typeparam>
	/// <typeparam name="TUrlProvider">the type of the REST URL provider</typeparam>
	/// <typeparam name="TRepository">the service type of the repository</typeparam>
	/// <typeparam name="TRepositoryImplementation">the implementation type of the repository</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddRestfulEntity<
		TEntity,
		TUrlProvider,
		TRepository,
		TRepositoryImplementation>(this IServiceCollection self)
		where TUrlProvider : class, IRestfulRepositoryUrlProvider<TEntity>
		where TRepository : class, IRepository<TEntity>
		where TRepositoryImplementation : class, TRepository
	{
		self.TryAddScoped<IRestfulRepositoryUrlProvider<TEntity>, TUrlProvider>();
		self.TryAddScoped<TRepository, TRepositoryImplementation>();

		if (typeof(TRepository) != typeof(IRepository<TEntity>))
		{
			self.AddScoped<IRepository<TEntity>>(
				sp => sp.GetRequiredService<TRepository>());
		}

		return self;
	}

	public static IServiceCollection AddRestClient(this IServiceCollection self)
	{
		self.TryAddSingleton<IRestClient, RestClient>();
		return self;
	}

	public static IServiceCollection AddRestClient<TClient>(this IServiceCollection self)
		where TClient : class, IRestClient
		=> self.AddSingleton<IRestClient, TClient>();
}