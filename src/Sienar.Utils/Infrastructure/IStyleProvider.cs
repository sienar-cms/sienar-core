using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// A container for <see cref="StyleResource"/> instances that should be rendered on each page
/// </summary>
public interface IStyleProvider : IList<StyleResource>;