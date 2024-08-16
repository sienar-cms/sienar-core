namespace Sienar.Infrastructure;

/// <summary>
/// Contains the data needed to create HTML <c>&lt;script&gt;</c> tags
/// </summary>
/// <remarks>
/// The <c>ScriptResource</c> class cannot be used to output inline JavaScript. Sienar does not support inline JavaScript.
/// </remarks>
public class ScriptResource
{
	/// <summary>
	/// The URL to the script resource
	/// </summary>
	/// <remarks>
	/// The URL provided here should either be absolute (e.g., to a CDN link) or root-relative (e.g., <c>/_content/My.Plugin.Assembly/main.js</c>).
	/// </remarks>
	public required string Src { get; set; }

	/// <summary>
	/// The name of the script resource
	/// </summary>
	/// <remarks>
	/// This name should reflect the name of the library, if applicable. For example, if the script resource is the JS file for Bootstrap, this property should be set to <c>"Bootstrap"</c>
	/// </remarks>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Whether the script resource should be included as a <b>module</b>
	/// </summary>
	public bool IsModule { get; set; }

	/// <summary>
	/// Whether the script resource should be included with <b>async</b>
	/// </summary>
	public bool IsAsync { get; set; }

	/// <summary>
	/// Whether the script resource should be included with <b>defer</b>
	/// </summary>
	public bool ShouldDefer { get; set; }

	/// <summary>
	/// The value to use for the <c>crossorigin</c> attribute
	/// </summary>
	public CrossOriginMode? CrossOriginMode { get; set; }

	/// <summary>
	/// The value to use for the <c>referrerpolicy</c> attribute
	/// </summary>
	public ReferrerPolicy? ReferrerPolicy { get; set; }

	/// <summary>
	/// The expected hash of the resource
	/// </summary>
	public string? Integrity { get; set; }

	/// <summary>
	/// Returns <c>"module"</c> if the script is an ES module, otherwise <c>null</c>
	/// </summary>
	public string? GetScriptType()
	{
		return IsModule
			? "module"
			: null;
	}

	/// <summary>
	/// Converts a string URL to a <c>ScriptResource</c>
	/// </summary>
	/// <param name="source">the URL of the script</param>
	/// <returns>the converted <c>ScriptResource</c></returns>
	public static implicit operator ScriptResource(string source)
		=> new() { Src = source };
}