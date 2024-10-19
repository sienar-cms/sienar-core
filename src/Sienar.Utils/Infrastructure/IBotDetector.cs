using Sienar.Data;

namespace Sienar.Infrastructure;

/// <summary>
/// (Hopefully) detects bots
/// </summary>
public interface IBotDetector
{
	/// <summary>
	/// Determines whether a honeypot has caught a bot
	/// </summary>
	/// <param name="honeypot">The honeypot</param>
	/// <returns>whether the honeypot caught a bot</returns>
	bool IsSpambot(Honeypot honeypot);
}
