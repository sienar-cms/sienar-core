using System;
using System.Reflection;
using Sienar.Data;

namespace Sienar.Extensions;

/// <summary>
/// Contains utilities that work on <see cref="EntityBase"/> instances or <c>typeof(TEntity)</c> instances
/// </summary>
public static class EntityExtensions
{
	/// <summary>
	/// Returns the human-friendly singular name of the entity
	/// </summary>
	/// <param name="self">the entity</param>
	/// <returns>the entity's name</returns>
	public static string GetEntityName(this EntityBase self)
		=> GetEntityName(self.GetType());

	/// <summary>
	/// Returns the human-friendly singular name of the entity
	/// </summary>
	/// <param name="self">the <see cref="Type"/> of the entity</param>
	/// <returns>the entity's name</returns>
	/// <remarks>
	/// If the specified <see cref="Type"/> does not have an <see cref="EntityNameAttribute"/> defined, then this method will return <c>Type.Name</c>. This behavior may be acceptable in some circumstances, but if it is not, be sure to only use entities with a defined <see cref="EntityNameAttribute"/>.
	/// </remarks>
	public static string GetEntityName(this Type self)
	{
		var attribute = self.GetCustomAttribute<EntityNameAttribute>();
		return attribute?.Singular ?? self.Name;
	}

	/// <summary>
	/// Returns the human-friendly plural name of the entity
	/// </summary>
	/// <param name="self">the entity</param>
	/// <returns>the entity's plural name</returns>
	public static string GetEntityPluralName(this EntityBase self)
		=> GetEntityPluralName(self.GetType());

	/// <summary>
	/// Returns the human-friendly plural name of the entity
	/// </summary>
	/// <param name="self">the <see cref="Type"/> of the entity</param>
	/// <returns>the entity's plural name</returns>
	/// <exception cref="InvalidOperationException">if no <see cref="EntityNameAttribute"/> is defined, or if no <c>EntityNameAttribute.Plural</c> value is provided</exception>
	public static string GetEntityPluralName(this Type self)
	{
		var attribute = self.GetCustomAttribute<EntityNameAttribute>();
		return attribute?.Plural
			?? throw new InvalidOperationException(
				$"Unable to determine plural entity name {self.Name}. "
				+ $"Please ensure you set the entity name with {nameof(EntityNameAttribute)}.");
	}
}