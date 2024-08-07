using System;

namespace Sienar.Infrastructure;

/// <summary>
/// Represents an HTML-friendly version of an enum member name
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class HtmlValueAttribute : Attribute
{
	/// <summary>
	/// The HTML-friendly value
	/// </summary>
	public string Value { get; }

	/// <summary>
	/// Creates a new instance of the <c>[HtmlValue]</c> attribute with the specified value
	/// </summary>
	/// <param name="value">the HTML-friendly value</param>
	public HtmlValueAttribute(string value) => Value = value;
}