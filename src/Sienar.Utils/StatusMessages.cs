#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using Sienar.Extensions;
using Sienar.Data;

namespace Sienar;

/// <exclude />
public static class StatusMessages
{
	public static class General
	{
		public const string Successful = "The operation was successful";
		public const string NotFound = "The requested resource was not found";
		public const string Unauthorized = "You must be logged in to perform that action";
		public const string Forbidden = "You do not have permission to perform that action";
		public const string Unprocessable = "There was a problem with the data you entered. Please check for errors and try again";
		public const string Conflict = "The data you entered conflicts with existing application data. Please check for errors and try again";
		public const string Concurrency = "Someone else updated the record before you. Please reload the page and try again";
		public const string Unknown = "An unknown error has occurred";
	}

	public static class Processes
	{
		public const string InvalidState = "Your request state is not valid. Please check your data and try again";
		public const string BeforeHookFailure = "One or more plugins failed to execute. Your operation could not be completed";
	}

	public static class Crud<TEntity> where TEntity : EntityBase
	{
		public static string CreateFailed() => $"Unable to create new {typeof(TEntity).GetEntityName()}";
		public static string CreateSuccessful() => $"{typeof(TEntity).GetEntityName()} created successfully";
		public static string ReadSingleFailed() => $"Unable to read {typeof(TEntity).GetEntityName()}";
		public static string ReadMultipleFailed() => $"Unable to read {typeof(TEntity).GetEntityPluralName()}";
		public static string UpdateFailed() => $"Unable to update {typeof(TEntity).GetEntityName()}";
		public static string UpdateSuccessful() => $"{typeof(TEntity).GetEntityName()} updated successfully";
		public static string DeleteFailed() => $"Unable to delete {typeof(TEntity).GetEntityName()}";
		public static string DeleteSuccessful() => $"{typeof(TEntity).GetEntityName()} deleted successfully";
		public static string NotFound(Guid id) => $"{typeof(TEntity).GetEntityName()} with ID {id} not found";
		public static string NoPermission() => $"You do not have permission to access this {typeof(TEntity).GetEntityName()}";
	}

	public static class Database
	{
		public const string QueryFailed = "Failed to query database";
	}

	public static class Rest
	{
		public const string NetworkFailed = "A network error occurred. Are you connected to the internet?";
		public const string NetworkTimeout = "Network request timed out";
		public const string BadRequest = "The request was malformed";
	}
}