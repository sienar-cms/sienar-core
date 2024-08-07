using System;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Data;

/// <summary>
/// A base class containing the fields required by all entities in the app with a customizable primary key type
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class EntityBase
{
	/// <summary>
	/// Represents the primary key of the entity
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// A unique value on the entity that ensures the entity is not modified concurrently
	/// </summary>
	public Guid ConcurrencyStamp { get; set; }
}