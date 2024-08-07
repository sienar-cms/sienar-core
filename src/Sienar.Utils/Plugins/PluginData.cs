using System;

namespace Sienar.Plugins;

/// <summary>
/// A data class that contains identifying information for plugins
/// </summary>
public class PluginData
{
	/// <summary>
	/// The name of the plugin. This should be globally unique
	/// </summary>
	public required string Name { get; init; } 

	/// <summary>
	/// The author of the plugin
	/// </summary>
	public required string Author { get; init; }

	/// <summary>
	/// The URL of the plugin author
	/// </summary>
	/// <remarks>
	/// This should be any URL that uniquely identifies the author. A company website, personal website, GitHub page, X profile, etc., would all be appropriate.
	/// </remarks>
	public required string AuthorUrl { get; init; }

	/// <summary>
	/// The <see cref="Version"/> of the plugin
	/// </summary>
	/// <remarks>
	/// This is not used by Sienar for any particular purpose, so technically speaking, you can set it once and forget it. However, we strongly recommend that you provide this value and update it as you update your plugin so your non-technical users can know which version of your plugin they are using.
	/// </remarks>
	public required Version Version { get; init; }

	/// <summary>
	/// A brief description of the plugin
	/// </summary>
	public required string Description { get; init; }

	/// <summary>
	/// The plugin's URL, if it has one. Defaults to <c>string.Empty</c>, indicating no URL exists
	/// </summary>
	/// <remarks>
	/// If the plugin has a URL, a link to the plugin's homepage will be generated on the plugins page. If not, no link will be generated. This value can safely be blank.
	/// </remarks>
	public string Homepage { get; set; } = string.Empty;
}