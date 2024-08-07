using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sienar.Configuration;
using Sienar.Extensions;

namespace Sienar.Utils.Tests.Extensions.SienarUtilsServiceCollectionExtensions;

public class ApplyDefaultConfiguration
{
	private const string SiteName = "test site";

	[Fact]
	public void NotConfigured_AppliesConfiguration()
	{
		// Arrange
		var options = new SienarOptions();
		var sut = new ServiceCollection();
		var configProvider = CreateConfiguration(SiteName);

		// Act
		sut.ApplyDefaultConfiguration<SienarOptions>(configProvider);

		// Assert
		(sut
			.First(s => s.ServiceType == typeof(IConfigureOptions<SienarOptions>))
			.ImplementationInstance as IConfigureOptions<SienarOptions>)!
			.Configure(options);

		options.SiteName
			.Should()
			.BeEquivalentTo(SiteName);
	}

	[Fact]
	public void AlreadyConfigured_DoesNothing()
	{
		// Arrange
		var options = new SienarOptions();
		var sut = new ServiceCollection();
		sut.Configure<SienarOptions>(o => o.SiteName = SiteName);
		var unusedConfigProvider = CreateConfiguration("Unused");

		// Act
		sut.ApplyDefaultConfiguration<SienarOptions>(unusedConfigProvider);

		// Assert
		(sut
			.First(s => s.ServiceType == typeof(IConfigureOptions<SienarOptions>))
			.ImplementationInstance as IConfigureOptions<SienarOptions>)!
			.Configure(options);

		options.SiteName
			.Should()
			.BeEquivalentTo(SiteName);
	}

	private static IConfiguration CreateConfiguration(string s)
	{
		var collection = new Dictionary<string, string?> { { "SiteName", s } };
		return new ConfigurationBuilder()
			.AddInMemoryCollection(collection)
			.Build();
	}
}