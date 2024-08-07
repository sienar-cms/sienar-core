using Sienar.Infrastructure;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Sienar.Infrastructure;

/// <exclude />
public class MenuProvider : DictionaryProvider<LinkDictionary<MenuLink>>,  IMenuProvider;