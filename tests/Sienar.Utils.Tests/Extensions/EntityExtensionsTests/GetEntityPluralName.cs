using System;
using Sienar.Extensions;
using Sienar.Data;

namespace Sienar.Utils.Tests.Extensions.EntityExtensionsTests;

public class GetEntityPluralName
{
	[Fact]
	public void EntityNameDefined_ReturnsPlural()
	{
		// Arrange
		const string expected = NamedEntity.PluralName;

		// Act
		var result = typeof(NamedEntity).GetEntityPluralName();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public void EntityNameUndefined_ThrowsInvalidOperationException()
	{
		// Arrange

		// Act
		var action = () => typeof(UnnamedEntity).GetEntityPluralName();

		// Assert
		action
			.Should()
			.Throw<InvalidOperationException>($"Unable to determine plural entity name {nameof(UnnamedEntity)}. Please ensure you set the entity name with {nameof(EntityNameAttribute)}.");
	}

	[Fact]
	public void InstancePassed_CallsOtherOverload()
	{
		// Arrange
		const string expected = NamedEntity.PluralName;
		var sut = new NamedEntity();

		// Act
		var result = sut.GetEntityPluralName();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}
}