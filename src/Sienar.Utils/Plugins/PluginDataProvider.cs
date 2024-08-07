#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Generic;

namespace Sienar.Plugins;

/// <exclude />
public class PluginDataProvider : List<PluginData>, IPluginDataProvider;