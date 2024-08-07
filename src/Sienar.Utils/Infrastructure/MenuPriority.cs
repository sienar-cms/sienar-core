namespace Sienar.Infrastructure;

/// <summary>
/// Represents the priority order in which menu items should be rendered
/// </summary>
public enum MenuPriority
{
	/// <summary>
	/// Menu items that should be rendered last
	/// </summary>
	Lowest,

	/// <summary>
	/// Menu items that should be rendered late, but not last
	/// </summary>
	Low,

	/// <summary>
	/// Menu items with no special priority
	/// </summary>
	Normal,

	/// <summary>
	/// Menu items that should be rendered early, but not first
	/// </summary>
	High,

	/// <summary>
	/// Menu items that should be rendered first
	/// </summary>
	Highest
}