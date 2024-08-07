// using Sienar.Extensions;
//
// namespace Sienar.Utils.Tests.Extensions.EnumExtensionsTests;
//
// public class GetHtmlValue
// {
// 	[Fact]
// 	public void HtmlValueDefined_ReturnsValue()
// 	{
// 		// Arrange
// 		const GrandMasters grandMaster = GrandMasters.MaceWindu;
// 		const string expected = "mace-windu";
//
// 		// Act
// 		var result = grandMaster.GetHtmlValue();
//
// 		// Assert
// 		result
// 			.Should()
// 			.BeEquivalentTo(expected);
// 	}
//
// 	[Fact]
// 	public void HtmlValueUndefined_ReturnsNull()
// 	{
// 		// Arrange
// 		const GrandMasters grandMaster = GrandMasters.BastilaShan;
// 		const string? expected = null;
//
// 		// Act
// 		var result = grandMaster.GetHtmlValue();
//
// 		// Assert
// 		result
// 			.Should()
// 			.BeEquivalentTo(expected);
// 	}
//
// 	[Fact]
// 	public void InvalidEnumMember_ReturnsNull()
// 	{
// 		// Arrange
// 		const GrandMasters grandMaster = (GrandMasters)5;
// 		const string? expected = null;
//
// 		// Act
// 		var result = grandMaster.GetHtmlValue();
//
// 		// Assert
// 		result
// 			.Should()
// 			.BeEquivalentTo(expected);
// 	}
//
// 	[Fact]
// 	public void NullEnumMember_ReturnsNull()
// 	{
// 		// Arrange
// 		GrandMasters? grandMaster = null;
// 		const string? expected = null;
//
// 		// Act
// 		var result = grandMaster.GetHtmlValue();
//
// 		// Assert
// 		result
// 			.Should()
// 			.BeEquivalentTo(expected);
// 	}
// }