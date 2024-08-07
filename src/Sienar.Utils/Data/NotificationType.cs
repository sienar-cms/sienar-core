namespace Sienar.Data;

/// <summary>
/// Represents different types of notifications
/// </summary>
public enum NotificationType
{
	/// <summary>
	/// A successful notification
	/// </summary>
	Success,

	/// <summary>
	/// A notification containing warning information
	/// </summary>
	Warning,

	/// <summary>
	/// A notification containing specific information
	/// </summary>
	Info,

	/// <summary>
	/// A notification indicating an error occurred
	/// </summary>
	Error
}