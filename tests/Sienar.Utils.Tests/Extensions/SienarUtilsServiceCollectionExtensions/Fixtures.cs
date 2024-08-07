using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Sienar.Utils.Tests.Extensions.SienarUtilsServiceCollectionExtensions;

internal class ServiceCollectionMock : Mock<IServiceCollection>
{
	private readonly ServiceDescriptor _serviceDescriptor;

	/// <inheritdoc />
	public ServiceCollectionMock(ServiceDescriptor serviceDescriptor)
	{
		_serviceDescriptor = serviceDescriptor;
		Setup(s => s.GetEnumerator())
			.Returns(GetEnumerator);
	}

	private IEnumerator<ServiceDescriptor> GetEnumerator()
	{
		yield return _serviceDescriptor;
	}
}