using System;

namespace Sienar.State;

/// <summary>
/// A base state provider that supports rudimentary change tracking and freezing/unfreezing to prevent unnecessary re-renders
/// </summary>
public class StateProviderBase
{
	/// <summary>
	/// Represents whether the state provider has queued changes waiting to be applied
	/// </summary>
	protected bool HasChanges;

	/// <summary>
	/// Represents whether the state provider should fire <see cref="OnChange"/> immediately
	/// </summary>
	protected bool IsFrozen;

	/// <summary>
	/// An event that fires when the state changes
	/// </summary>
	public event Action? OnChange;

	/// <summary>
	/// Notify subscribers to the <see cref="OnChange"/> event that the state has changed
	/// </summary>
	/// <remarks>
	/// This method does not necessarily invoke <see cref="OnChange"/>. If the state provider is frozen, it instead sets <see cref="HasChanges"/> to <c>true</c> and returns early. In this way, when the <see cref="Unfreeze"/> method is called, all subscribers can be notified once, avoiding unnecessary re-renders when multiple properties change.
	/// </remarks>
	protected void NotifyStateChanged()
	{
		if (IsFrozen)
		{
			HasChanges = true;
			return;
		}

		OnChange?.Invoke();
	}

	/// <summary>
	/// Freezes the state provider, preventing it from notifying <see cref="OnChange"/> subscribers of changes until the <see cref="Unfreeze"/> method is called
	/// </summary>
	public void Freeze() => IsFrozen = true;

	/// <summary>
	/// Unfreezes the state provider and invokes <see cref="OnChange"/> if any changes were made
	/// </summary>
	public void Unfreeze()
	{
		IsFrozen = false;

		if (HasChanges)
		{
			HasChanges = false;
			NotifyStateChanged();
		}
	}
}