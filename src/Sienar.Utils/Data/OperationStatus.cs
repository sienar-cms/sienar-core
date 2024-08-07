namespace Sienar.Data;

/// <summary>
/// Represents different statuses with which a hook or process can exit
/// </summary>
public enum OperationStatus
{
	/// <summary>
	/// Indicates that a hook or process was successful
	/// </summary>
	Success,

	/// <summary>
	/// Indicates that the operation failed due to a missing entity
	/// </summary>
	NotFound,

	/// <summary>
	/// Indicates that the operation failed because the user was not logged in
	/// </summary>
	Unauthorized,

	/// <summary>
	/// Indicates that the operation failed because the user was not allowed to perform the operation
	/// </summary>
	Forbidden,

	/// <summary>
	/// Indicates that the operation failed due to the entity or request state
	/// </summary>
	Unprocessable,

	/// <summary>
	/// Indicates that the operation failed due to a database conflict
	/// </summary>
	Conflict,

	/// <summary>
	/// Indicates that the operation failed due to a database concurrency error
	/// </summary>
	Concurrency,

	/// <summary>
	/// Indicates that the operation failed for unknown reasons
	/// </summary>
	Unknown
}