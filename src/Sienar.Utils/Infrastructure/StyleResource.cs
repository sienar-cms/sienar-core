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
	public required string Href { get; set; }

	/// <summary>
	/// The name of the style resource
	/// </summary>
	/// <remarks>
	/// This name should reflect the name of the library, if applicable. For example, if the style resource is the CSS file for Bootstrap, this property should be set to <c>"Bootstrap"</c>
	/// </remarks>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The value to use for the <c>crossorigin</c> attribute
	/// </summary>
	public CrossOriginMode? CrossOriginMode { get; set; }

	/// <summary>
	/// The value to u se for the <c>referrerpolicy</c> attribute
	/// </summary>
	public ReferrerPolicy? ReferrerPolicy { get; set; }

	/// <summary>
	/// The expected hash of the resource
	/// </summary>
	public string? Integrity { get; set; }

	/// <summary>
	/// Converts a string URL to a <c>StyleResource</c>
	/// </summary>
	/// <param name="source">the URL of the stylesheet</param>
	/// <returns>the converted <c>StyleResource</c></returns>
	public static implicit operator StyleResource(string source)
		=> new() { Href = source };
}