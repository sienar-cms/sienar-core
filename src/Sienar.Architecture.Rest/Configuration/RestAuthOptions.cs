namespace Sienar.Configuration;

public class RestAuthOptions
{
	/// <summary>
	/// The URL used to authenticate to a REST API
	/// </summary>
	public required string AuthenticationUrl { get; set; }
}