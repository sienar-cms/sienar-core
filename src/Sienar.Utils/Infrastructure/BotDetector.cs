#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.Extensions.Logging;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <exclude />
public class BotDetector : IBotDetector
{
	private readonly ILogger<BotDetector> _logger;

	public BotDetector(ILogger<BotDetector> logger)
	{
		_logger = logger;
	}

	public bool IsSpambot(Honeypot honeypot)
	{
		_logger.LogInformation(
			"Form submission completed in {time} seconds",
			honeypot.TimeToComplete.TotalSeconds);

		var isSpambot = !string.IsNullOrEmpty(honeypot.SecretKeyField);

		if (isSpambot)
		{
			_logger.LogError(
				"Spambot detected! Value passed to honeypot was '{value}'",
				honeypot.SecretKeyField);
		}

		return isSpambot;
	}
}
