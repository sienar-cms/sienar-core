using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sienar.Data;

/// <summary>
/// Used to search, sort, order, and page through database records
/// </summary>
public class Filter
{
	/// <summary>
	/// The text value to search for, if any
	/// </summary>
	[ExcludeFromCodeCoverage]
	public string? SearchTerm { get; set; }

	/// <summary>
	/// The name of the column to sort by, if any
	/// </summary>
	[ExcludeFromCodeCoverage]
	public string? SortName { get; set; }

	/// <summary>
	/// Whether to explicitly sort by value descending
	/// </summary>
	[ExcludeFromCodeCoverage]
	public bool? SortDescending { get; set; }

	/// <summary>
	/// The page of results to return (1-indexed)
	/// </summary>
	[ExcludeFromCodeCoverage]
	public int Page { get; set; } = 1;

	/// <summary>
	/// The number of results in a single page
	/// </summary>
	[ExcludeFromCodeCoverage]
	public int PageSize { get; set; } = 5;

	/// <summary>
	/// The related entities to include
	/// </summary>
	[ExcludeFromCodeCoverage]
	public List<string> Includes { get; set; } = [];

	/// <summary>
	/// Creates a <c>Filter</c> that returns all entities instead of paging them
	/// </summary>
	/// <returns>the new filter</returns>
	public static Filter GetAll() => new() { PageSize = 0 };

	/// <summary>
	/// Creates a <c>Filter</c> that 
	/// </summary>
	/// <param name="includes"></param>
	/// <returns></returns>
	public static Filter WithIncludes(params string[] includes) => new() { Includes = [..includes] };
}