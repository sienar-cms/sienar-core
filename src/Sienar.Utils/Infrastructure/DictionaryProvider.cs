#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <exclude />
public class DictionaryProvider<T> : Dictionary<string, T>, IDictionaryProvider<T>
	where T : new()
{
	public T Access(string name)
	{
		if (!TryGetValue(name, out var item))
		{
			item = new();
			this[name] = item;
		}

		return item;
	}
}