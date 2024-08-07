using Sienar.Infrastructure;

namespace Sienar.Infrastructure;

/// <summary>
/// The <see cref="IDictionaryProvider{T}"/> used to contain <see cref="DashboardLink">dashboard links</see>
/// </summary>
public interface IDashboardProvider : IDictionaryProvider<LinkDictionary<DashboardLink>>;