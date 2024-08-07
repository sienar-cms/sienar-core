namespace Sienar.Data;

/// <summary>
/// Represents a serialized notification for transmission across a REST API
/// </summary>
public class Notification
{
	/// <summary>
	/// The message of the notification
	/// </summary>
	public string Message { get; }

	/// <summary>
	/// The type of the notification
	/// </summary>
	public NotificationType Type { get; }

	/// <summary>
	/// Creates a new instance of the <c>Notification</c> class
	/// </summary>
	/// <param name="message">the message of the notification</param>
	/// <param name="type">the type of the notification</param>
	public Notification(string message, NotificationType type)
	{
		Message = message;
		Type = type;
	}
}