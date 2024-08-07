namespace Sienar.Infrastructure;

/// <summary>
/// Contains the data needed to create an HTML <c>&lt;link&gt;</c> tag
/// </summary>
/// <remarks>
/// The <c>StyleResource</c> cannot be used to output non-stylesheet links or internal CSS. Sienar does not support internal CSS.
/// </remarks>
public class StyleResource
{
	/// <summary>
	/// The URL to the script resource
	/// </summary>
	/// <remarks>
	/// The URL provided here should either be absolute (e.g., to a CDN link) or root-relative (e.g., <c>/_content/My.Plugin.Assembly/main.js</c>).
	/// </remarks>
	public required string Href { get; init; }

	/// <summary>
	/// The value to use for the <c>crossorigin</c> attribute
	/// </summary>
	public CrossOriginMode? CrossOriginMode { get; init; }

	/// <summary>
	/// The value to u se for the <c>referrerpolicy</c> attribute
	/// </summary>
	public ReferrerPolicy? ReferrerPolicy { get; init; }

	/// <summary>
	/// The expected hash of the resource
	/// </summary>
	public string? Integrity { get; init; }

	/// <summary>
	/// Converts a string URL to a <c>StyleResource</c>
	/// </summary>
	/// <param name="source">the URL of the stylesheet</param>
	/// <returns>the converted <c>StyleResource</c></returns>
	public static implicit operator StyleResource(string source)
		=> new() { Href = source };
}