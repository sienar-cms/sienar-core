using Sienar.Infrastructure;

namespace Sienar.Utils.Tests.Infrastructure;

public class AccessValidationContextTests
{
	[Fact]
	public void ValidationContext_ByDefault_DeniesAccess()
	{
		// Arrange
		var sut = new AccessValidationContext();

		// Act
		var canAccess = sut.CanAccess;

		// Assert
		canAccess
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ValidationContext_WhenApproved_GrantsAccess()
	{
		// Arrange
		var sut = new AccessValidationContext();

		// Act
		sut.Approve();
		var canAccess = sut.CanAccess;

		// Assert
		canAccess
			.Should()
			.BeTrue();
	}
}