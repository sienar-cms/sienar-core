using Sienar.Extensions;

namespace Sienar.Utils.Tests.Extensions.EntityExtensionsTests;

public class GetEntityName
{
	[Fact]
	public void EntityNameDefined_ReturnsSingular()
	{
		// Arrange
		const string expected = NamedEntity.SingularName;

		// Act
		var result = typeof(NamedEntity).GetEntityName();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public void EntityNameUndefined_ReturnsTypeName()
	{
		// Arrange
		const string expected = nameof(UnnamedEntity);

		// Act
		var result = typeof(UnnamedEntity).GetEntityName();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public void InstancePassed_CallsOtherOverload()
	{
		// Arrange
		const string expected = nameof(UnnamedEntity);
		var sut = new UnnamedEntity();

		// Act
		var result = sut.GetEntityName();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}
}