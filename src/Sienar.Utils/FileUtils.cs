using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Sienar;

/// <summary>
/// Contains several utilities for accessing the file system
/// </summary>
[ExcludeFromCodeCoverage]
public static class FileUtils
{
	/// <summary>
	/// Contains the base file system directory name for the application
	/// </summary>
	/// <remarks>
	/// This property is static, so be aware that changes to its value will propagate across the entire application and may cause errors if not done carefully.
	/// </remarks>
	public static string BaseAppFolderName { get; set; } = "";

	/// <summary>
	/// Returns the path of the local application data directory
	/// </summary>
	/// <returns>the path</returns>
	public static string GetLocalAppDataDirectory()
		=> Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

	/// <summary>
	/// Returns the path of the user's desktop directory
	/// </summary>
	/// <returns>the path</returns>
	public static string GetDesktopDirectory()
		=> Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

	/// <summary>
	/// Returns the provided subpath as a combined path appended to the <see cref="BaseAppFolderName"/>
	/// </summary>
	/// <param name="subpath">the subpath to append</param>
	/// <returns>the path</returns>
	public static string GetBaseRelativePath(string subpath)
		=> Path.Combine(BaseAppFolderName, subpath);

	/// <summary>
	/// Checks if the given directory name exists, and if not, creates it
	/// </summary>
	/// <param name="dirname">the directory to create if it does not exist</param>
	public static void EnsureDirectoryExists(string dirname)
	{
		if (!Directory.Exists(dirname))
		{
			Directory.CreateDirectory(dirname);
		}
	}
}