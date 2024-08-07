using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Extensions;

namespace Sienar.Utils.Tests.Extensions.SienarUtilsServiceCollectionExtensions;

public class GetAndRemoveService
{
	private const string Value1 = "value 1";

	[Fact]
	public void ServiceExists_ReturnsInstance()
	{
		// Arrange
		List<string> service = [Value1];
		var sut = new ServiceCollection();
		sut.AddSingleton<IList<string>>(service);

		// Act
		var result = (IList<string>?)sut.GetAndRemoveService(typeof(IList<string>));

		// Assert
		result
			.Should()
			.NotBeNull();
		result
			.Should()
			.HaveCount(1);
		result
			.Should()
			.HaveElementAt(0, Value1);
	}

	[Fact]
	public void ServiceNull_ReturnsNull()
	{
		// Arrange
		var sut = new ServiceCollection();

		// Act
		var result = sut.GetAndRemoveService(typeof(IList<string>));

		// Assert
		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void GenericTypeArgPassed_CallsOtherOverload()
	{
		// Arrange
		List<string> service = [Value1];
		var sut = new ServiceCollection();
		sut.AddSingleton<IList<string>>(service);

		// Act
		var result = sut.GetAndRemoveService<IList<string>>();

		// Assert
		result
			.Should()
			.NotBeNull();
	}
}