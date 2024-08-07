using System;
using Microsoft.Extensions.Configuration;

namespace Sienar.Infrastructure;

/// <summary>
/// Used by Sienar to configure a service or middleware that is generally configured by an <see cref="Action{T}"/>
/// </summary>
/// <typeparam name="TOptions">the type of the configuration options class to configure</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IConfigurer<TOptions> where TOptions : class
{
	/// <summary>
	/// Configures an instance of <c>T</c>
	/// </summary>
	/// <param name="options">the instance of <c>T</c> to configure</param>
	/// <param name="config">the configuration container</param>
	void Configure(
		TOptions options,
		IConfiguration config);
}