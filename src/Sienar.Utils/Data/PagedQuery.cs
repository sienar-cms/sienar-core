using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Data;

/// <summary>
/// Used to page through database results
/// </summary>
/// <typeparam name="TModel"></typeparam>
[ExcludeFromCodeCoverage]
public class PagedQuery<TModel>
{
	/// <summary>
	/// Represents the items in the current page of results
	/// </summary>
	public IEnumerable<TModel> Items { get; set; } = Array.Empty<TModel>();

	/// <summary>
	/// Represents the total number of items across all pages of results
	/// </summary>
	public int TotalCount { get; set; }

	/// <summary>
	/// Creates an empty <c>PagedQuery</c>
	/// </summary>
	public PagedQuery() {}

	/// <summary>
	/// Creates a <c>PagedQuery</c> that has been populated with results
	/// </summary>
	/// <param name="items">the results on the current page</param>
	/// <param name="totalCount">the total number of items in the database that match the query</param>
	public PagedQuery(IEnumerable<TModel> items, int totalCount)
	{
		Items = items;
		TotalCount = totalCount;
	}
}