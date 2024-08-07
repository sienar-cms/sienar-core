using Sienar.Extensions;

namespace Sienar.Utils.Tests.Extensions.EnumExtensionsTests;

public class GetDescription
{
	[Fact]
	public void DescriptionDefined_ReturnsValue()
	{
		// Arrange
		const GrandMasters grandMaster = GrandMasters.MaceWindu;
		const string expected = "Jedi Grand Master during the Clone Wars";

		// Act
		var result = grandMaster.GetDescription();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public void HtmlValueUndefined_ReturnsStringName()
	{
		// Arrange
		const GrandMasters grandMaster = GrandMasters.BastilaShan;
		const string expected = "BastilaShan";

		// Act
		var result = grandMaster.GetDescription();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}

	[Fact]
	public void InvalidEnumMember_ReturnsNonMemberValue()
	{
		// Arrange
		const GrandMasters grandMaster = (GrandMasters)5;
		const string expected = "5";

		// Act
		var result = grandMaster.GetDescription();

		// Assert
		result
			.Should()
			.BeEquivalentTo(expected);
	}
}