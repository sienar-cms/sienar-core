namespace Sienar.Infrastructure;

/// <summary>
/// Represents possible values of the <c>referrerpolicy</c> HTML attribute
/// </summary>
/// <remarks>
/// Sienar uses this enum when creating script and style resources. For information on individual values of the <c>referrerpolicy</c> attribute, see <see href="https://developer.mozilla.org/en-US/docs/Web/API/HTMLScriptElement/referrerPolicy">referrerpolicy on MDN</see>
/// </remarks>
public enum ReferrerPolicy
{
	/// <summary>
	/// Represents <c>referrerpolicy="no-referrer"</c>
	/// </summary>
	[HtmlValue("no-referrer")]
	NoReferrer,

	/// <summary>
	/// Represents <c>referrerpolicy="no-referrer-when-downgrade"</c>
	/// </summary>
	[HtmlValue("no-referrer-when-downgrade")]
	NoReferrerWhenDowngrade,

	/// <summary>
	/// Represents <c>referrerpolicy="origin"</c>
	/// </summary>
	[HtmlValue("origin")]
	Origin,

	/// <summary>
	/// Represents <c>referrerpolicy="origin-when-cross-origin"</c>
	/// </summary>
	[HtmlValue("origin-when-cross-origin")]
	OriginWhenCrossOrigin,

	/// <summary>
	/// Represents <c>referrerpolicy="same-origin"</c>
	/// </summary>
	[HtmlValue("same-origin")]
	SameOrigin,

	/// <summary>
	/// Represents <c>referrerpolicy="strict-origin"</c>
	/// </summary>
	[HtmlValue("strict-origin")]
	StrictOrigin,

	/// <summary>
	/// Represents <c>referrerpolicy="strict-origin-when-cross-origin"</c>
	/// </summary>
	[HtmlValue("strict-origin-when-cross-origin")]
	StrictOriginWhenCrossOrigin,

	/// <summary>
	/// Represents <c>referrerpolicy="unsafe-url"</c>
	/// </summary>
	[HtmlValue("unsafe-url")]
	UnsafeUrl
}