using System.Collections.Generic;

namespace Sienar.Plugins;

/// <summary>
/// A container for <see cref="PluginData"/> that can be used to display information to end users about which plugins are active in the app
/// </summary>
public interface IPluginDataProvider : IList<PluginData>;