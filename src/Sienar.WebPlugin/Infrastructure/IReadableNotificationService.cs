using System.Collections.Generic;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <summary>
/// A version of <see cref="INotificationService"/> that allows developers to retrieve the underlying notifications so they can be shown to users
/// </summary>
public interface IReadableNotificationService : INotificationService
{
	/// <summary>
	/// The list of notifications registered in the notification service
	/// </summary>
	List<Notification> Notifications { get; }
}