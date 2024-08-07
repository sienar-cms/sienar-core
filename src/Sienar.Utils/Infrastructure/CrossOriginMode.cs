namespace Sienar.Infrastructure;

/// <summary>
/// Represents the possible values of the <c>crossorigin</c> HTML attribute
/// </summary>
/// <remarks>
/// Sienar uses this enum when creating script and style resources. For information on individual values of the <c>crossorigin</c> attribute, see <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/crossorigin">crossorigin on MDN</see>
/// </remarks>
public enum CrossOriginMode
{
	/// <summary>
	/// Represents <c>crossorigin=""</c>
	/// </summary>
	[HtmlValue("")]
	None,

	/// <summary>
	/// Represents <c>crossorigin="anonymous"</c>
	/// </summary>
	[HtmlValue("anonymous")]
	Anonymous,

	/// <summary>
	/// Represents <c>crossorigin="use-credentials"</c>
	/// </summary>
	[HtmlValue("use-credentials")]
	UseCredentials
}