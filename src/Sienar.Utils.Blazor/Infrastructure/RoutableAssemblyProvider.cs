#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Generic;
using System.Reflection;

namespace Sienar.Infrastructure;

/// <exclude />
public class RoutableAssemblyProvider : List<Assembly>, IRoutableAssemblyProvider;