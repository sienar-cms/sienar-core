using System;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Data;

/// <summary>
/// Allows developers to define singular and plural names for their entities in a way that Sienar can understand
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public class EntityNameAttribute : Attribute
{
	/// <summary>
	/// Defines the singular name for an entity (e.g., "pet")
	/// </summary>
	public required string Singular { get; set; }

	/// <summary>
	/// Defines the plural name for an entity (e.g., "pets")
	/// </summary>
	public required string Plural { get; set; }
}