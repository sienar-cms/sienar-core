using System;
using Microsoft.AspNetCore.Components;

namespace Sienar.Infrastructure;

/// <summary>
/// Identifies an item as a service that should be loaded by the <see cref="OwningComponentBase"/> service provider
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ServiceAttribute : Attribute;