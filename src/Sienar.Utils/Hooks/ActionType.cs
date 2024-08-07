namespace Sienar.Hooks;

/// <summary>
/// The <see cref="ActionType"/> enum represents different types of actions that can be performed by services
/// </summary>
public enum ActionType
{
	/// <summary>
	/// Represents a CRUD action that reads a single entity
	/// </summary>
	Read,

	/// <summary>
	/// Represents a CRUD action that reads multiple entities
	/// </summary>
	ReadAll,

	/// <summary>
	/// Represents a CRUD action that creates a new entity
	/// </summary>
	Create,

	/// <summary>
	/// Represents a CRUD action that updates an existing entity
	/// </summary>
	Update,

	/// <summary>
	/// Represents a CRUD action that deletes an existing entity
	/// </summary>
	Delete,

	/// <summary>
	/// Represents a general action that accepts an input and returns an output on success
	/// </summary>
	Action,

	/// <summary>
	/// Represents a general action that accepts an input and returns a <c>bool</c> to indicate its success status
	/// </summary>
	StatusAction,

	/// <summary>
	/// Represents a general action that accepts no input and returns an output on success
	/// </summary>
	ResultAction
}