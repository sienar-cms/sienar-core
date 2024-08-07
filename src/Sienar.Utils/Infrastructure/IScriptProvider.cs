using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// A container for <see cref="ScriptResource"/> instances that should be rendered on each page
/// </summary>
public interface IScriptProvider : IList<ScriptResource>;