using System;
using System.Threading.Tasks;
using Sienar.Infrastructure;
using Sienar.Services;

namespace Sienar.Data;

/// <summary>
/// An <see cref="IRepository{TEntity}"/> that supports REST API datastores
/// </summary>
/// <typeparam name="TEntity">the type of the entity</typeparam>
public class RestfulRepository<TEntity> : IRepository<TEntity>
	where TEntity : EntityBase
{
	private readonly IRestClient _client;
	private readonly INotificationService _notifier;
	private readonly IRestfulRepositoryUrlProvider<TEntity> _urlProvider;

	public RestfulRepository(
		IRestClient client,
		INotificationService notifier,
		IRestfulRepositoryUrlProvider<TEntity> urlProvider)
	{
		_client = client;
		_notifier = notifier;
		_urlProvider = urlProvider;
	}

	/// <inheritdoc />
	public async Task<TEntity?> Read(Guid id, Filter? filter = null)
	{
		var response = await _client.Get<WebResult<TEntity>>(
			_urlProvider.GenerateReadUrl(id),
			filter);
		EmitNotifications(response);
		return response.Result?.Result;
	}

	/// <inheritdoc />
	public async Task<PagedQuery<TEntity>> Read(Filter? filter = null)
	{
		var response = await _client.Get<WebResult<PagedQuery<TEntity>>>(
			_urlProvider.GenerateReadUrl(),
			filter);
		EmitNotifications(response);
		return response.Result?.Result ?? new();
	}

	/// <inheritdoc />
	public async Task<Guid> Create(TEntity entity)
	{
		var response = await _client.Post<WebResult<Guid>>(
			_urlProvider.GenerateCreateUrl(entity),
			entity);
		EmitNotifications(response);
		return response.Result?.Result ?? Guid.Empty;
	}

	/// <inheritdoc />
	public async Task<bool> Update(TEntity entity)
	{
		var response = await _client.Put<WebResult<bool>>(
			_urlProvider.GenerateUpdateUrl(entity),
			entity);
		EmitNotifications(response);
		return response.Result?.Result ?? false;
	}

	/// <inheritdoc />
	public async Task<bool> Delete(Guid id)
	{
		var response = await _client.Delete<WebResult<bool>>(_urlProvider.GenerateDeleteUrl(id));
		EmitNotifications(response);
		return response.Result?.Result ?? false;
	}

	/// <inheritdoc />
	public async Task<Guid?> ReadConcurrencyStamp(Guid id)
	{
		var response = await _client.Get<WebResult<TEntity>>(_urlProvider.GenerateReadUrl(id));
		EmitNotifications(response);
		return response.Result?.Result.ConcurrencyStamp;
	}

	/// <summary>
	/// Emits notifications from an <see cref="WebResult{TResult}"/>
	/// </summary>
	/// <param name="result">the API result</param>
	private void EmitNotifications<TResult>(OperationResult<WebResult<TResult>> result)
	{
		if (result.Result is null)
		{
			var message = result.Status switch
			{
				OperationStatus.NotFound => StatusMessages.General.NotFound,
				OperationStatus.Unauthorized => StatusMessages.General.Unauthorized,
				OperationStatus.Unprocessable => StatusMessages.General.Unprocessable,
				OperationStatus.Conflict => StatusMessages.General.Conflict,
				OperationStatus.Concurrency => StatusMessages.General.Concurrency,
				OperationStatus.Success => "Server returned an empty response",
				_ => StatusMessages.General.Unknown
			};

			_notifier.Error(message);
			return;
		}

		foreach (var notification in result.Result.Notifications)
		{
			switch (notification.Type)
			{
				case NotificationType.Success:
					_notifier.Success(notification.Message);
					break;
				case NotificationType.Info:
					_notifier.Info(notification.Message);
					break;
				case NotificationType.Warning:
					_notifier.Warning(notification.Message);
					break;
				case NotificationType.Error:
				default:
					_notifier.Error(notification.Message);
					break;
			}
		}
	}
}