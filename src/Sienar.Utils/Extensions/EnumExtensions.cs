using System;
using System.ComponentModel;
using System.Reflection;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains utilities that work on enums
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// Gets the value of the <see cref="DescriptionAttribute"/> if defined, or else the name of the enum member as a string
	/// </summary>
	/// <param name="self">the enum field</param>
	/// <returns>the description if defined, else the stringified name of the enum member</returns>
	public static string GetDescription(this Enum self)
	{
		var stringified = self.ToString();
		var a = self
			.GetType()
			.GetField(stringified)
			?.GetCustomAttribute<DescriptionAttribute>();
		return a?.Description ?? stringified;
	}

	/// <summary>
	/// Gets the HTML-expected value of <see cref="ReferrerPolicy"/> and <see cref="CrossOriginMode"/> members
	/// </summary>
	/// <param name="self">the referrer policy or cross-origin mode to get a value for</param>
	/// <returns>the value if the enum is not null and the value is defined, else null</returns>
	public static string? GetHtmlValue(this Enum? self)
	{
		return self?
			.GetType()
			.GetField(self.ToString())?
			.GetCustomAttribute<HtmlValueAttribute>()
			?.Value;
	}
}