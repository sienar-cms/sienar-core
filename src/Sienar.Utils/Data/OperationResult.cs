namespace Sienar.Data;

/// <summary>
/// Represents the result of a hookable operation
/// </summary>
/// <typeparam name="TResult">the type of the result</typeparam>
public class OperationResult<TResult>
{
	/// <summary>
	/// The status of the operation
	/// </summary>
	public readonly OperationStatus Status;

	/// <summary>
	/// The value returned from the operation
	/// </summary>
	public readonly TResult? Result;

	/// <summary>
	/// The status message from the operation
	/// </summary>
	public readonly string Message;

	/// <summary>
	/// Creates a new instance of <c>OperationResult&lt;TResult&gt;</c>
	/// </summary>
	/// <param name="status">the status of the operation</param>
	/// <param name="result">the value returned from the operation</param>
	/// <param name="message">the status message from the operation</param>
	public OperationResult(
		OperationStatus status = OperationStatus.Success,
		TResult? result = default,
		string? message = null)
	{
		Status = status;
		Result = result;
		Message = message ?? CreateDefaultMessage(status);
	}

	private static string CreateDefaultMessage(
		OperationStatus status) => status switch
	{
		OperationStatus.Success => StatusMessages.General.Successful,
		OperationStatus.NotFound => StatusMessages.General.NotFound,
		OperationStatus.Unauthorized => StatusMessages.General.Unauthorized,
		OperationStatus.Forbidden => StatusMessages.General.Forbidden,
		OperationStatus.Unprocessable => StatusMessages.General.Unprocessable,
		OperationStatus.Conflict => StatusMessages.General.Conflict,
		OperationStatus.Concurrency => StatusMessages.General.Concurrency,
		_ => StatusMessages.General.Unknown
	};
}