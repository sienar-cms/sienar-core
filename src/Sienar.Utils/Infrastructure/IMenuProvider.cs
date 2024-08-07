using Sienar.Infrastructure;

namespace Sienar.Infrastructure;

/// <summary>
/// The <see cref="IDictionaryProvider{T}"/> used to contain <see cref="MenuLink">menu links</see>
/// </summary>
public interface IMenuProvider : IDictionaryProvider<LinkDictionary<MenuLink>>;