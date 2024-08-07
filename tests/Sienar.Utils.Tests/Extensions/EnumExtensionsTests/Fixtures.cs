using System.ComponentModel;
using Sienar.Plugins;

namespace Sienar.Utils.Tests.Extensions.EnumExtensionsTests;

internal enum GrandMasters
{
	[Description("Jedi Grand Master during the Clone Wars")]
	// [HtmlValue("mace-windu")]
	MaceWindu,
	BastilaShan
}