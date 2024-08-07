using System.Collections.Generic;

namespace Sienar.Infrastructure;

// ReSharper disable once TypeParameterCanBeVariant
/// <summary>
/// A wrapper around a dictionary that guarantees that an item with the specified string key exists prior to access
/// </summary>
/// <typeparam name="T">the type of the value portion of the dictionary</typeparam>
public interface IDictionaryProvider<T> : IDictionary<string, T>
{
	/// <summary>
	/// Returns an item to operate on
	/// </summary>
	/// <param name="name">The name of the  specific item</param>
	/// <returns>the item</returns>
	T Access(string name);
}