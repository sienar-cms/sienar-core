namespace Sienar.Data;

/// <summary>
/// Contains the information from a REST API request
/// </summary>
public class WebResult<TResult>
{
	/// <summary>
	/// The result of a REST API request
	/// </summary>
	public TResult? Result { get; set; }

	/// <summary>
	/// Notifications generated during the request
	/// </summary>
	public required Notification[] Notifications { get; set; }
}