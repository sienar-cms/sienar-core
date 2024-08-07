using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceProvider"/> extension methods for the <c>Sienar.Utils</c> assembly
/// </summary>
public static class SienarSqliteUtilsServiceProviderExtensions
{
	/// <summary>
	/// Migrates a SQLite-based EntityFramework <c>DbContext</c> with automatic rollback if errors occur
	/// </summary>
	/// <param name="self">the service provider</param>
	/// <param name="dbPath">the string path to the SQLite database file</param>
	/// <typeparam name="TContext">the type of the <c>DbContext</c></typeparam>
	/// <exception cref="Exception">if an exception occurs during migration</exception>
	public static void MigrateDb<TContext>(
		this IServiceProvider self,
		string dbPath)
		where TContext : DbContext
	{
		var backupPath = $"{dbPath}.backup";
		var enableBackup = File.Exists(dbPath);

		// Make backup of existing database
		if (enableBackup)
		{
			File.Copy(dbPath, backupPath, true);
		}

		// Perform migration
		try
		{
			using var scope = self.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
			var migrator = dbContext.Database.GetService<IMigrator>();
			migrator.Migrate();
		}
		catch (Exception e)
		{
			if (enableBackup)
			{
				File.Copy(backupPath, dbPath, true);
				File.Delete(backupPath);
			}

			throw new Exception($"Database {dbPath} failed to update", e);
		}

		// Migration was successful, so delete backup
		if (enableBackup)
		{
			File.Delete(backupPath);
		}
	}

	/// <summary>
	/// Configures a <c>DbContext</c> to use SQLite
	/// </summary>
	/// <param name="self">the <see cref="DbContextOptionsBuilder"/></param>
	/// <param name="source">the string path to the SQLite database file</param>
	public static void UseSqliteDb(
		this DbContextOptionsBuilder self,
		string source)
	{
		self.UseSqlite($"Data Source={source}");
	}
}