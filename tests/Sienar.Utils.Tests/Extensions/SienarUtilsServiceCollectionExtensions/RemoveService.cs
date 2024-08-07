using Microsoft.Extensions.DependencyInjection;
using Sienar.Extensions;

namespace Sienar.Utils.Tests.Extensions.SienarUtilsServiceCollectionExtensions;

public class RemoveService
{
	private readonly ServiceDescriptor _serviceDescriptor = new(typeof(string), null, "");

	[Fact]
	public void ServiceExists_RemovesService()
	{
		// Arrange
		var mock = new ServiceCollectionMock(_serviceDescriptor);

		var sut = mock.Object;

		// Act
		sut.RemoveService(typeof(string));

		// Assert
		mock.Verify(
			s => s.Remove(_serviceDescriptor),
			Times.Once);
	}

	[Fact]
	public void ServiceNotExists_DoesNotCallRemove()
	{
		// Arrange
		var mock = new ServiceCollectionMock(_serviceDescriptor);

		var sut = mock.Object;

		// Act
		sut.RemoveService(typeof(bool));

		// Assert
		mock.Verify(
			s => s.Remove(It.IsAny<ServiceDescriptor>()),
			Times.Never);
	}

	[Fact]
	public void GenericTypeArgPassed_CallsOtherOverload()
	{
		// Arrange
		var mock = new ServiceCollectionMock(_serviceDescriptor);

		var sut = mock.Object;

		// Act
		sut.RemoveService<string>();

		// Assert
		mock.Verify(
			s => s.Remove(_serviceDescriptor),
			Times.Once);
	}
}