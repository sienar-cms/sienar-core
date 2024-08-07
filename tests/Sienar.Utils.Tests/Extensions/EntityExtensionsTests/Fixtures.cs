using Sienar.Data;

namespace Sienar.Utils.Tests.Extensions.EntityExtensionsTests;

[EntityName(Singular = SingularName, Plural = PluralName)]
internal class NamedEntity : EntityBase
{
	public const string SingularName = "Name";
	public const string PluralName = "Names";
}

internal class UnnamedEntity : EntityBase;