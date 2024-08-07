using System.Diagnostics.CodeAnalysis;

namespace Sienar.Configuration;

/// <summary>
/// Configures core Sienar options
/// </summary>
[ExcludeFromCodeCoverage]
public class SienarOptions
{
	/// <summary>
	/// Whether Sienar should send email notifications or not
	/// </summary>
	public bool EnableEmail { get; set; }

	/// <summary>
	/// Whether Sienar should allow the registration of new users or not
	/// </summary>
	public bool RegistrationOpen { get; set; }

	/// <summary>
	/// The name of the Sienar application
	/// </summary>
	public string SiteName { get; set; } = string.Empty;

	/// <summary>
	/// The URL of the Sienar application
	/// </summary>
	public string SiteUrl { get; set; } = string.Empty;
}